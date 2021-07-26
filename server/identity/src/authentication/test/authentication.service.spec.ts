import { ConfigModule } from '@nestjs/config';
import { JwtModule } from '@nestjs/jwt';
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
import { ProfileService } from '../../profile/profile.service';
import { getProfileStub } from '../../../test/support/stubs/profile.stub';

const profileMock = getProfileStub();

describe('AuthenticationService', () => {
  let service: AuthenticationService;
  let profileService: ProfileService;
  let connection: Connection;

  beforeEach(async () => {
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
    profileService = module.get<ProfileService>(ProfileService);
    connection = await module.get(getConnectionToken());
  });

  afterAll(async (done) => {
    await connection.close();
    await closeMongoConnection();
    done();
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('createJwtFromMagicMetadata', () => {
    it('should call findByEmail', async () => {
      const findSpy = spyOn(profileService, 'findByEmail').and.returnValue({
        email: profileMock.email,
      });

      await service.createJwtFromMagicMetadata({
        email: profileMock.email,
        publicAddress: 'address',
        issuer: 'issuer',
      });

      expect(findSpy).toBeCalledTimes(1);
    });
  });
});
