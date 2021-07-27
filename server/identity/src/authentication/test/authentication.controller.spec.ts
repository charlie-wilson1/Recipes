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

describe('IdentityController', () => {
  let controller: AuthenticationController;
  let service: AuthenticationService;
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

    controller = module.get<AuthenticationController>(AuthenticationController);
    service = await module.get<AuthenticationService>(AuthenticationService);
    connection = await module.get(getConnectionToken());
  });

  afterAll(async (done) => {
    await connection.close();
    await closeMongoConnection();
    done();
  });

  test('should be defined', () => {
    expect(controller).toBeDefined();
  });

  describe('authenticate', () => {
    test('should call authenticateDidToken and createJwtFromMagicMetadata', async () => {
      const authenticateSpy = spyOn(service, 'authenticateDidToken');
      const createTokenSpy = spyOn(service, 'createJwtFromMagicMetadata');

      await controller.authenticate({
        didToken: 'test',
      });

      expect(authenticateSpy).toBeCalledTimes(1);
      expect(createTokenSpy).toBeCalledTimes(1);
    });
  });

  describe('logout', () => {
    test('should call logout', async () => {
      const logoutSpy = spyOn(service, 'logout');

      await controller.logout({
        didToken: 'test',
      });

      expect(logoutSpy).toBeCalledTimes(1);
    });
  });
});
