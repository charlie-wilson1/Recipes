import { ConfigModule } from '@nestjs/config';
import { JwtModule, JwtService } from '@nestjs/jwt';
import { PassportModule } from '@nestjs/passport';
import { Test, TestingModule } from '@nestjs/testing';
import { ProfileModule } from '../../profile/profile.module';
import { AuthenticationController } from '../authentication.controller';
import { AuthenticationService } from '../authentication.service';
import { JwtStrategy } from '../strategies/jwt.strategy';
import testDbConfig, { closeMongoConnection } from '../../../test/testDbConfig';
import { Connection } from 'mongoose';
import { getConnectionToken } from '@nestjs/mongoose';
import * as Joi from 'joi';
import { getProfileStub } from '../../../test/support/stubs/profile.stub';
import { ProfileRepository } from '../../profile/profile.repository';
import { MagicUserMetadata } from '@magic-sdk/admin';
import { UnauthorizedException } from '@nestjs/common';

const profileMock = getProfileStub();

describe('AuthenticationService', () => {
  let service: AuthenticationService;
  let profileRepository: ProfileRepository;
  let jwtService: JwtService;
  let connection: Connection;

  beforeEach(async (done) => {
    const module: TestingModule = await Test.createTestingModule({
      controllers: [AuthenticationController],
      imports: [
        ConfigModule.forRoot({
          validationSchema: Joi.object({
            JWT_SECRET: Joi.string().default('secret'),
            JWT_ISSUER: Joi.string().default('issuer'),
          }),
        }),
        JwtModule.registerAsync({
          useFactory: async () => ({
            secretOrPrivateKey: 'test',
            signOptions: {
              expiresIn: '60s',
              issuer: 'issuer',
            },
          }),
        }),
        testDbConfig({
          connectionName: (new Date().getTime() * Math.random()).toString(16),
        }),
        PassportModule,
        ProfileModule,
      ],
      providers: [AuthenticationService, JwtStrategy],
    }).compile();

    service = module.get<AuthenticationService>(AuthenticationService);
    profileRepository = module.get<ProfileRepository>(ProfileRepository);
    jwtService = module.get<JwtService>(JwtService);
    connection = await module.get(getConnectionToken());
    jest.clearAllMocks();
    done();
  });

  afterEach(async (done) => {
    await connection.close();
    await closeMongoConnection();
    done();
  });

  test('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('createJwtFromMagicMetadata', () => {
    let findByEmailSpy: jest.SpyInstance;
    let signSpy: jest.SpyInstance;
    const metadata: MagicUserMetadata = {
      email: profileMock.email,
      publicAddress: 'address',
      issuer: 'issuer',
    };

    beforeEach((done) => {
      findByEmailSpy = jest.spyOn(profileRepository, 'findByEmail');
      signSpy = jest.spyOn(jwtService, 'signAsync');
      done();
    });

    test('should throw UnauthorizedException when user not found', async () => {
      findByEmailSpy.mockImplementation(() => Promise.resolve());

      try {
        await service.createJwtFromMagicMetadata(metadata);
      } catch (error) {
        expect(error).toBeInstanceOf(UnauthorizedException);
        expect(error.message).toBe(
          `user with email ${metadata.email} is not authorized to use this application. Please contact an administrator to be invited to use the application.`,
        );
      }

      expect(profileRepository.findByEmail).toHaveBeenCalledWith(
        profileMock.email,
      );
      expect(findByEmailSpy).toHaveBeenCalledTimes(1);
      expect(signSpy).not.toHaveBeenCalled();
    });

    test('should call findByEmail', async () => {
      findByEmailSpy.mockImplementation(() => Promise.resolve(profileMock));
      signSpy.mockImplementation(() => Promise.resolve());

      await service.createJwtFromMagicMetadata(metadata);

      expect(profileRepository.findByEmail).toHaveBeenCalledWith(
        profileMock.email,
      );

      expect(jwtService.signAsync).toHaveBeenCalledWith({
        sub: metadata.issuer,
        publicAddress: metadata.publicAddress,
        roles: profileMock.roles,
        name: profileMock.username,
      });

      expect(findByEmailSpy).toHaveBeenCalledTimes(1);
      expect(signSpy).toHaveBeenCalledTimes(1);
    });
  });
});
