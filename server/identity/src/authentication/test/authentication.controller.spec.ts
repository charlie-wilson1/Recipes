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
import { getProfileStub } from '../../profile/test/support/stubs/profile.stub';
import { AuthRequestDto } from '../models/loginRequestDto';
import { MagicUserMetadata } from '@magic-sdk/admin';
import { UnauthorizedException } from '@nestjs/common';

const profileMock = getProfileStub();
const didToken: AuthRequestDto = { didToken: 'test' };

describe('IdentityController', () => {
  let controller: AuthenticationController;
  let service: AuthenticationService;
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

    controller = module.get<AuthenticationController>(AuthenticationController);
    service = await module.get<AuthenticationService>(AuthenticationService);
    connection = await module.get(getConnectionToken());
    done();
  });

  afterEach(async (done) => {
    await connection.close();
    await closeMongoConnection();
    done();
  });

  test('should be defined', () => {
    expect(controller).toBeDefined();
  });

  describe('authenticate', () => {
    let authenticateSpy: jest.SpyInstance;
    let logoutSpy: jest.SpyInstance;
    let getProfileSpy: jest.SpyInstance;
    let createTokenSpy: jest.SpyInstance;

    const expectedMetadata: MagicUserMetadata = {
      email: profileMock.email,
      publicAddress: 'publicAddress',
      issuer: 'issuer',
    };

    beforeEach((done) => {
      authenticateSpy = jest.spyOn(service, 'authenticateDidToken');
      getProfileSpy = jest.spyOn(service, 'getProfile');
      logoutSpy = jest.spyOn(service, 'logout');
      createTokenSpy = jest.spyOn(service, 'createJwtFromProfile');
      done();
    });

    test('should throw UnauthorizedException if metadata invalid', async () => {
      authenticateSpy.mockImplementation(() => Promise.resolve(undefined));
      logoutSpy.mockImplementation();

      try {
        await controller.authenticate(didToken);
      } catch (error) {
        expect(error).toBeInstanceOf(UnauthorizedException);
      }

      expect(service.authenticateDidToken).toHaveBeenCalledWith(
        didToken.didToken,
      );
      expect(service.logout).toHaveBeenCalledWith(didToken.didToken);
      expect(authenticateSpy).toHaveBeenCalledTimes(1);
      expect(logoutSpy).toHaveBeenCalledTimes(1);
      expect(createTokenSpy).not.toHaveBeenCalled();
    });

    test('should throw UnauthorizedException if metadata invalid', async () => {
      authenticateSpy.mockImplementation(() =>
        Promise.resolve(expectedMetadata),
      );
      getProfileSpy.mockImplementation(() =>
        Promise.resolve({ ...profileMock, isActive: false }),
      );
      logoutSpy.mockImplementation();

      try {
        await controller.authenticate(didToken);
      } catch (error) {
        expect(error).toBeInstanceOf(UnauthorizedException);
      }

      expect(service.authenticateDidToken).toHaveBeenCalledWith(
        didToken.didToken,
      );

      expect(service.getProfile).toHaveBeenCalledWith(expectedMetadata.email);
      expect(service.logout).toHaveBeenCalledWith(didToken.didToken);
      expect(authenticateSpy).toHaveBeenCalledTimes(1);
      expect(getProfileSpy).toHaveBeenCalledTimes(1);
      expect(logoutSpy).toHaveBeenCalledTimes(1);
      expect(createTokenSpy).not.toHaveBeenCalled();
    });

    test('should call authenticateDidToken and createJwtFromMagicMetadata', async () => {
      const expectedResult = 'jwt';

      authenticateSpy.mockImplementation(() =>
        Promise.resolve(expectedMetadata),
      );
      getProfileSpy.mockImplementation(() => Promise.resolve(profileMock));
      createTokenSpy.mockImplementation(() => Promise.resolve(expectedResult));

      const actual = await controller.authenticate(didToken);

      expect(service.authenticateDidToken).toHaveBeenCalledWith(
        didToken.didToken,
      );
      expect(service.getProfile).toHaveBeenCalledWith(expectedMetadata.email);
      expect(service.createJwtFromProfile).toHaveBeenCalledWith(profileMock);
      expect(actual).toEqual(expectedResult);
      expect(authenticateSpy).toHaveBeenCalledTimes(1);
      expect(createTokenSpy).toHaveBeenCalledTimes(1);
    });
  });

  describe('logout', () => {
    test('should call logout', async () => {
      const logoutSpy = jest
        .spyOn(service, 'logout')
        .mockImplementation(() => Promise.resolve());

      await controller.logout(didToken);

      expect(service.logout).toHaveBeenCalledWith(didToken.didToken);
      expect(logoutSpy).toHaveBeenCalledTimes(1);
    });
  });
});
