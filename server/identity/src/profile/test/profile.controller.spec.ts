import { Test, TestingModule } from '@nestjs/testing';
import { ProfileController } from '../profile.controller';
import { ProfileService } from '../profile.service';
import { getProfileStub } from '../../../test/support/stubs/profile.stub';
import { ProfileRepository } from '../profile.repository';
import { AuthenticationController } from '../../authentication/authentication.controller';
import { ConfigModule } from '@nestjs/config';
import { JwtModule } from '@nestjs/jwt';
import {
  MongooseModule,
  getModelToken,
  getConnectionToken,
} from '@nestjs/mongoose';
import { PassportModule } from '@nestjs/passport';
import * as Joi from 'joi';
import { AuthenticationService } from '../../authentication/authentication.service';
import { JwtStrategy } from '../../authentication/strategies/jwt.strategy';
import testDbConfig, { closeMongoConnection } from '../../../test/testDbConfig';
import { Profile, ProfileSchema } from '../models/profile.schema';
import { ProfileModule } from '../profile.module';
import { ProfileModelMock } from './support/profileMock.model';
import { Connection } from 'mongoose';
import { GetAllDto } from '../models/getAllDto';
import { UpdateRolesDto } from '../models/updateRolesDto';

const profileMock = getProfileStub();

describe('ProfileController', () => {
  let controller: ProfileController;
  let service: ProfileService;
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
        MongooseModule.forFeature([
          {
            name: Profile.name,
            schema: ProfileSchema,
          },
        ]),
        PassportModule,
        ProfileModule,
      ],
      providers: [
        ProfileService,
        ProfileRepository,
        AuthenticationService,
        JwtStrategy,
        {
          provide: getModelToken(Profile.name),
          useClass: ProfileModelMock,
        },
      ],
    }).compile();

    controller = module.get<ProfileController>(ProfileController);
    service = module.get<ProfileService>(ProfileService);
    connection = await module.get(getConnectionToken());
    jest.resetAllMocks();
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

  describe('create', () => {
    test('should call create', async () => {
      const spy = jest
        .spyOn(service, 'create')
        .mockImplementation(() => Promise.resolve(profileMock));

      const actual = await controller.create({
        email: profileMock.email,
      });

      expect(service.create).toHaveBeenCalledWith({ email: profileMock.email });
      expect(spy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(profileMock);
    });
  });

  describe('getAll', () => {
    test('should call getAll', async () => {
      const spy = jest
        .spyOn(service, 'getAll')
        .mockImplementation(() => Promise.resolve([profileMock]));

      const expectedRequest: GetAllDto = {
        paginatedRequest: {
          limit: 10,
          offset: 0,
        },
        isActive: true,
      };

      const actual = await controller.getAll(expectedRequest);

      expect(service.getAll).toHaveBeenCalledWith(expectedRequest);
      expect(spy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual([profileMock]);
    });
  });

  describe('getByEmail', () => {
    test('should call findByEmail', async () => {
      const spy = jest
        .spyOn(service, 'findByEmail')
        .mockImplementation(() => Promise.resolve(profileMock));

      const actual = await controller.getByEmail({
        email: profileMock.email,
      });

      expect(service.findByEmail).toHaveBeenCalledWith({
        email: profileMock.email,
      });
      expect(spy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(profileMock);
    });
  });

  describe('getByUsername', () => {
    test('should call findByUsername', async () => {
      const spy = jest
        .spyOn(service, 'findByUsername')
        .mockImplementation(() => Promise.resolve(profileMock));

      const actual = await controller.getByUsername({
        username: profileMock.username,
      });

      expect(service.findByUsername).toHaveBeenCalledTimes(1);
      expect(spy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(profileMock);
    });
  });

  describe('delete', () => {
    test('should call delete', async () => {
      const spy = jest
        .spyOn(service, 'delete')
        .mockImplementation(() => Promise.resolve());

      await controller.delete({
        email: profileMock.email,
      });

      expect(service.delete).toHaveBeenCalledWith({ email: profileMock.email });
      expect(spy).toHaveBeenCalledTimes(1);
    });
  });

  describe('updateRoles', () => {
    test('should call updateRoles', async () => {
      const spy = jest
        .spyOn(service, 'updateRoles')
        .mockImplementation(() => Promise.resolve(profileMock));

      const expectedRequest: UpdateRolesDto = {
        email: profileMock.email,
        roles: profileMock.roles,
      };

      const actual = await controller.updateRoles(expectedRequest);

      expect(service.updateRoles).toHaveBeenCalledWith(expectedRequest);
      expect(spy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(profileMock);
    });
  });

  describe('updateUsername', () => {
    test('should call updateRoles', async () => {
      jest
        .spyOn(service, 'updateProfileUsername')
        .mockImplementation(() => Promise.resolve(profileMock));

      const actual = await controller.updateUsername({
        email: profileMock.email,
        username: profileMock.username,
      });

      expect(service.updateProfileUsername).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(profileMock);
    });
  });
});
