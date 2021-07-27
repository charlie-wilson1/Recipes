import { ConfigModule } from '@nestjs/config';
import { JwtModule } from '@nestjs/jwt';
import {
  getConnectionToken,
  getModelToken,
  MongooseModule,
} from '@nestjs/mongoose';
import { PassportModule } from '@nestjs/passport';
import { Test, TestingModule } from '@nestjs/testing';
import * as Joi from 'joi';
import { Connection } from 'mongoose';
import { AuthenticationController } from '../../authentication/authentication.controller';
import { AuthenticationService } from '../../authentication/authentication.service';
import { JwtStrategy } from '../../authentication/strategies/jwt.strategy';
import testDbConfig, { closeMongoConnection } from '../../../test/testDbConfig';
import { Profile, ProfileSchema } from '../models/profile.schema';
import { ProfileModule } from '../profile.module';
import { ProfileService } from '../profile.service';
import { ProfileModelMock } from './support/profileMock.model';
import { getProfileStub } from '../../../test/support/stubs/profile.stub';
import { ProfileRepository } from '../profile.repository';
import { ConflictException, NotFoundException } from '@nestjs/common';
import { GetAllDto } from '../models/getAllDto';
import {
  FindProfileByEmailDto,
  FindProfileByUsernameDto,
} from '../models/findProfileDtos';
import { UpdateUsernameDto } from '../models/updateUsernameDto';

const profileMock = getProfileStub();

describe('ProfileService', () => {
  let service: ProfileService;
  let repository: ProfileRepository;
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

    service = module.get<ProfileService>(ProfileService);
    repository = module.get<ProfileRepository>(ProfileRepository);
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
    expect(service).toBeDefined();
  });

  describe('create', () => {
    let findByEmailSpy: jest.SpyInstance;
    let createSpy: jest.SpyInstance;

    beforeEach((done) => {
      findByEmailSpy = jest.spyOn(repository, 'findByEmail');
      createSpy = jest.spyOn(repository, 'create');
      done();
    });

    test('should throw ConflictException when existing user with email found', async () => {
      findByEmailSpy.mockImplementation(() => Promise.resolve(profileMock));
      try {
        await service.create({ email: profileMock.email });
      } catch (error) {
        expect(error).toBeInstanceOf(ConflictException);
        expect(error.message).toBe(
          `profile with email ${profileMock.email} already exists.`,
        );
      }

      expect(repository.findByEmail).toHaveBeenCalledWith(profileMock.email);
      expect(repository.create).not.toHaveBeenCalled();
      expect(findByEmailSpy).toHaveBeenCalledTimes(1);
      expect(createSpy).not.toHaveBeenCalled();
    });

    test('should return created user', async () => {
      findByEmailSpy.mockImplementation(() => Promise.resolve());
      createSpy.mockImplementation(() => Promise.resolve(profileMock));

      const actual = await service.create({ email: profileMock.email });

      expect(repository.findByEmail).toHaveBeenCalledWith(profileMock.email);
      expect(repository.create).toHaveBeenCalledWith(profileMock.email);
      expect(findByEmailSpy).toHaveBeenCalledTimes(1);
      expect(createSpy).toHaveBeenCalledTimes(1);
      expect(actual).toBe(profileMock);
    });
  });

  describe('getAll', () => {
    let getAllSpy: jest.SpyInstance;

    const expectedRequestDto: GetAllDto = {
      paginatedRequest: {
        limit: 10,
        offset: 0,
      },
      isActive: true,
    };

    beforeEach((done) => {
      getAllSpy = jest.spyOn(repository, 'getAll');
      done();
    });

    test('should throw NotFoundException when no existing users not found', async () => {
      getAllSpy.mockImplementation(() => Promise.resolve());

      try {
        await service.getAll(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe('No profiles found.');
      }

      expect(repository.getAll).toHaveBeenCalledWith(
        expectedRequestDto.paginatedRequest,
        expectedRequestDto.isActive,
      );
      expect(getAllSpy).toHaveBeenCalledTimes(1);
    });

    test('should return user', async () => {
      getAllSpy.mockImplementation(() => Promise.resolve([profileMock]));
      const actual = await service.getAll(expectedRequestDto);

      expect(repository.getAll).toHaveBeenCalledWith(
        expectedRequestDto.paginatedRequest,
        expectedRequestDto.isActive,
      );

      expect(getAllSpy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual([profileMock]);
    });
  });

  describe('findByEmail', () => {
    let findByEmailSpy: jest.SpyInstance;

    const expectedRequestDto: FindProfileByEmailDto = {
      email: profileMock.email,
    };

    beforeEach((done) => {
      findByEmailSpy = jest.spyOn(repository, 'findByEmail');
      done();
    });

    test('should throw NotFoundException user with email not found', async () => {
      findByEmailSpy.mockImplementation(() => Promise.resolve());

      try {
        await service.findByEmail(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe(
          `User with email ${profileMock.email} could not be found`,
        );
      }

      expect(repository.findByEmail).toHaveBeenCalledWith(
        expectedRequestDto.email,
      );
      expect(findByEmailSpy).toHaveBeenCalledTimes(1);
    });

    test('should return user', async () => {
      findByEmailSpy.mockImplementation(() => Promise.resolve(profileMock));
      const actual = await service.findByEmail(expectedRequestDto);

      expect(repository.findByEmail).toHaveBeenCalledWith(
        expectedRequestDto.email,
      );

      expect(findByEmailSpy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(profileMock);
    });
  });

  describe('findByUsername', () => {
    let findByUsernameSpy: jest.SpyInstance;

    const expectedRequestDto: FindProfileByUsernameDto = {
      username: profileMock.username,
    };

    beforeEach((done) => {
      findByUsernameSpy = jest.spyOn(repository, 'findByUsername');
      done();
    });

    test('should throw NotFoundException user with username not found', async () => {
      findByUsernameSpy.mockImplementation(() => Promise.resolve());

      try {
        await service.findByUsername(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe(
          `User with username ${profileMock.username} could not be found`,
        );
      }

      expect(repository.findByUsername).toHaveBeenCalledWith(
        expectedRequestDto.username,
      );
      expect(findByUsernameSpy).toHaveBeenCalledTimes(1);
    });

    test('should return user', async () => {
      findByUsernameSpy.mockImplementation(() => Promise.resolve(profileMock));
      const actual = await service.findByUsername(expectedRequestDto);

      expect(repository.findByUsername).toHaveBeenCalledWith(
        expectedRequestDto.username,
      );

      expect(findByUsernameSpy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(profileMock);
    });
  });

  describe('delete', () => {
    let findByEmailSpy: jest.SpyInstance;
    let deleteSpy: jest.SpyInstance;

    const expectedRequestDto: FindProfileByEmailDto = {
      email: profileMock.email,
    };

    beforeEach((done) => {
      findByEmailSpy = jest.spyOn(repository, 'findByEmail');
      deleteSpy = jest.spyOn(repository, 'delete');
      done();
    });

    test('should throw NotFoundException when user with email not found', async () => {
      findByEmailSpy.mockImplementation(() => Promise.resolve());

      try {
        await service.findByEmail(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe(
          `User with email ${profileMock.email} could not be found`,
        );
      }

      expect(repository.findByEmail).toHaveBeenCalledWith(
        expectedRequestDto.email,
      );
      expect(findByEmailSpy).toHaveBeenCalledTimes(1);
    });

    test('should call delete', async () => {
      const profileToDelete = {
        ...profileMock,
        id: '1',
      };

      findByEmailSpy.mockImplementation(() => Promise.resolve(profileToDelete));
      deleteSpy.mockImplementation(() => Promise.resolve());

      await service.delete(expectedRequestDto);

      expect(repository.findByEmail).toHaveBeenCalledWith(
        expectedRequestDto.email,
      );

      expect(findByEmailSpy).toHaveBeenCalledTimes(1);
      expect(repository.delete).toHaveBeenCalledWith(profileToDelete.id);
    });
  });

  describe('updateRoles', () => {
    let findByUsernameNotEmailSpy: jest.SpyInstance;
    let findByEmailSpy: jest.SpyInstance;
    let updateSpy: jest.SpyInstance;

    const expectedRequestDto: UpdateUsernameDto = {
      email: profileMock.email,
      username: `${profileMock.username}_2`,
    };

    beforeEach((done) => {
      findByUsernameNotEmailSpy = jest.spyOn(
        repository,
        'findByUsernameNotEmail',
      );
      findByEmailSpy = jest.spyOn(repository, 'findByEmail');
      updateSpy = jest.spyOn(repository, 'update');
      done();
    });

    test('should throw ConflictException when another existing user with username found', async () => {
      findByUsernameNotEmailSpy.mockImplementation(() =>
        Promise.resolve([profileMock]),
      );

      try {
        await service.updateProfileUsername(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(ConflictException);
        expect(error.message).toBe(
          `profile with username ${expectedRequestDto.username} already exists.`,
        );
      }

      expect(repository.findByUsernameNotEmail).toHaveBeenCalledWith(
        expectedRequestDto.username,
        expectedRequestDto.email,
      );
      expect(repository.findByEmail).not.toHaveBeenCalled();
      expect(repository.update).not.toHaveBeenCalled();
      expect(findByUsernameNotEmailSpy).toHaveBeenCalledTimes(1);
      expect(findByEmailSpy).not.toHaveBeenCalled();
      expect(updateSpy).not.toHaveBeenCalled();
    });

    test('should throw NotFoundException when user with email not found', async () => {
      findByUsernameNotEmailSpy.mockImplementation(() => Promise.resolve());
      findByEmailSpy.mockImplementation(() => Promise.resolve());

      try {
        await service.updateProfileUsername(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe(
          `User with email ${profileMock.email} could not be found`,
        );
      }

      expect(repository.findByUsernameNotEmail).toHaveBeenCalledWith(
        expectedRequestDto.username,
        expectedRequestDto.email,
      );
      expect(repository.findByEmail).toHaveBeenCalledWith(
        expectedRequestDto.email,
      );
      expect(repository.update).not.toHaveBeenCalled();
      expect(findByUsernameNotEmailSpy).toHaveBeenCalledTimes(1);
      expect(findByEmailSpy).toHaveBeenCalledTimes(1);
      expect(updateSpy).not.toHaveBeenCalled();
    });

    test('should return user', async () => {
      const expectedResponse = {
        ...profileMock,
        username: expectedRequestDto.username,
      };

      findByUsernameNotEmailSpy.mockImplementation(() => Promise.resolve());
      findByEmailSpy.mockImplementation(() =>
        Promise.resolve({ ...profileMock, id: '1' }),
      );
      updateSpy.mockImplementation(() => Promise.resolve(expectedResponse));

      const actual = await service.updateProfileUsername(expectedRequestDto);

      expect(repository.findByUsernameNotEmail).toHaveBeenCalledWith(
        expectedRequestDto.username,
        expectedRequestDto.email,
      );
      expect(repository.findByEmail).toHaveBeenCalledWith(
        expectedRequestDto.email,
      );
      expect(repository.update).toHaveBeenCalledWith('1', {
        ...expectedResponse,
        id: '1',
      });

      expect(findByUsernameNotEmailSpy).toHaveBeenCalledTimes(1);
      expect(findByEmailSpy).toHaveBeenCalledTimes(1);
      expect(updateSpy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(expectedResponse);
    });
  });
});
