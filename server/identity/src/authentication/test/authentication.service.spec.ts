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
import { getProfileStub } from '../../profile/test/support/stubs/profile.stub';
import { ProfileRepository } from '../../profile/profile.repository';

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

  describe('getProfile', () => {
    let findByEmailSpy: jest.SpyInstance;

    beforeEach((done) => {
      findByEmailSpy = jest.spyOn(profileRepository, 'findByEmail');
      done();
    });

    test('should call findByEmail', async () => {
      findByEmailSpy.mockImplementation(() => Promise.resolve(profileMock));
      await service.getProfile(profileMock.email);
      expect(findByEmailSpy).toHaveBeenCalledTimes(1);

      expect(profileRepository.findByEmail).toHaveBeenCalledWith(
        profileMock.email,
      );
    });
  });

  describe('createJwtFromProfile', () => {
    let signSpy: jest.SpyInstance;

    beforeEach((done) => {
      signSpy = jest.spyOn(jwtService, 'sign');
      done();
    });

    test('should sign jwt with username and roles', async () => {
      const expectedResult = 'jwtToken';
      signSpy.mockImplementation(() => expectedResult);
      const actual = service.createJwtFromProfile(profileMock);
      expect(signSpy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(expectedResult);

      expect(jwtService.sign).toHaveBeenCalledWith({
        roles: profileMock.roles,
        name: profileMock.username,
        nameid: profileMock.email,
      });
    });
  });
});
