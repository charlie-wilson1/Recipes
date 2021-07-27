import { getConnectionToken, MongooseModule } from '@nestjs/mongoose';
import { Test, TestingModule } from '@nestjs/testing';
import { Connection } from 'mongoose';
import testDbConfig, { closeMongoConnection } from '../../../test/testDbConfig';
import { Profile, ProfileSchema } from '../models/profile.schema';
import { ProfileController } from '../profile.controller';
import { ProfileService } from '../profile.service';
import { getProfileStub } from '../../../test/support/stubs/profile.stub';

const profileMock = getProfileStub();

describe('ProfileController', () => {
  let controller: ProfileController;
  let service: ProfileService;
  let connection: Connection;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      imports: [
        testDbConfig({
          connectionName: (new Date().getTime() * Math.random()).toString(16),
        }),
        MongooseModule.forFeature([
          {
            name: Profile.name,
            schema: ProfileSchema,
          },
        ]),
      ],
      providers: [ProfileService],
      controllers: [ProfileController],
      exports: [ProfileService],
    }).compile();

    controller = module.get<ProfileController>(ProfileController);
    service = await module.get<ProfileService>(ProfileService);
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

  describe('create', () => {
    test('should call create', () => {
      spyOn(service, 'create');

      controller.create({
        email: profileMock.email,
      });

      expect(service.create).toHaveBeenCalledTimes(1);
    });
  });

  describe('getAll', () => {
    test('should call getAll', () => {
      spyOn(service, 'getAll');

      controller.getAll({
        limit: 10,
        offset: 0,
      });

      expect(service.getAll).toHaveBeenCalledTimes(1);
    });
  });

  describe('getByEmail', () => {
    test('should call findByEmail', () => {
      spyOn(service, 'findByEmail');

      controller.getByEmail({
        email: profileMock.email,
      });

      expect(service.findByEmail).toHaveBeenCalledTimes(1);
    });
  });

  describe('getByUsername', () => {
    test('should call findByUsername', () => {
      spyOn(service, 'findByUsername');

      controller.getByUsername({
        username: profileMock.username,
      });

      expect(service.findByUsername).toHaveBeenCalledTimes(1);
    });
  });

  describe('delete', () => {
    test('should call delete', () => {
      spyOn(service, 'delete');

      controller.delete({
        email: profileMock.email,
      });

      expect(service.delete).toHaveBeenCalledTimes(1);
    });
  });

  describe('updateRoles', () => {
    test('should call updateRoles', () => {
      spyOn(service, 'updateRoles');

      controller.updateRoles({
        email: profileMock.email,
        roles: profileMock.roles,
      });

      expect(service.updateRoles).toHaveBeenCalledTimes(1);
    });
  });

  describe('updateUsername', () => {
    test('should call updateRoles', () => {
      spyOn(service, 'updateProfileUsername');

      controller.updateUsername({
        email: profileMock.email,
        username: profileMock.username,
      });

      expect(service.updateProfileUsername).toHaveBeenCalledTimes(1);
    });
  });
});
