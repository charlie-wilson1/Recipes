import { Test, TestingModule } from '@nestjs/testing';
import { UserService } from '../user.service';
import { UserRepository } from '../user.repository';
import {
  ConflictException,
  LoggerService,
  NotFoundException,
} from '@nestjs/common';
import { GetUsersDto } from '../dtos/getUsers.dto';
import { FindUserByEmailDto } from '../dtos/findUserByEmail.dto';
import { FindUserByUsernameDto } from '../dtos/FindUserByUsername.dto';
import { UpdateUsernameDto } from '../dtos/updateUsername.dto';
import { User } from '../entities/user.entity';
import { Role } from '../../models/roles';
import { WINSTON_MODULE_NEST_PROVIDER } from 'nest-winston';
import { UpdateRolesDto } from '../dtos/updateRoles.dto';

const mockUserRepository = () => ({
  findOne: jest.fn(),
  createUser: jest.fn(),
  findByUsernameNotEmail: jest.fn(),
  findUsersPaginated: jest.fn(),
  softDelete: jest.fn(),
  save: jest.fn(),
});

const mockUser: User = {
  _id: '1',
  username: 'username',
  roles: [Role.Member],
  email: 'test@test.com',
  isActive: true,
};

describe('UserService', () => {
  let service: UserService;
  let repository: UserRepository;
  let logger: LoggerService;

  beforeEach(async (done) => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        UserService,
        { provide: UserRepository, useFactory: mockUserRepository },
        {
          provide: WINSTON_MODULE_NEST_PROVIDER,
          useFactory: () => require('winston'),
        },
      ],
    }).compile();

    service = module.get<UserService>(UserService);
    repository = module.get<UserRepository>(UserRepository);
    logger = module.get<LoggerService>(WINSTON_MODULE_NEST_PROVIDER);
    jest.resetAllMocks();
    done();
  });

  test('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('create', () => {
    let findOneSpy: jest.SpyInstance;
    let createUserSpy: jest.SpyInstance;
    let logSpy: jest.SpyInstance;

    beforeEach((done) => {
      findOneSpy = jest.spyOn(repository, 'findOne');
      createUserSpy = jest.spyOn(repository, 'createUser');
      logSpy = jest.spyOn(logger, 'log');
      done();
    });

    test('should throw ConflictException when existing user with email found', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve(mockUser));

      try {
        await service.create({ email: mockUser.email });
      } catch (error) {
        expect(error).toBeInstanceOf(ConflictException);
        expect(error.message).toBe(
          `user with email ${mockUser.email} already exists.`,
        );
      }

      expect(repository.findOne).toHaveBeenCalledWith({
        email: mockUser.email,
      });
      expect(logger.log).toHaveBeenCalledWith(
        `create: User with email ${mockUser.email} already exists.`,
        UserService.name,
      );

      expect(findOneSpy).toHaveBeenCalledTimes(1);
      expect(logSpy).toHaveBeenCalledTimes(1);
      expect(createUserSpy).not.toHaveBeenCalled();
    });

    test('should return created user', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve());
      createUserSpy.mockImplementation(() => Promise.resolve(mockUser));

      const actual = await service.create({ email: mockUser.email });

      expect(repository.findOne).toHaveBeenCalledWith({
        email: mockUser.email,
      });
      expect(repository.createUser).toHaveBeenCalledWith(mockUser.email);

      expect(findOneSpy).toHaveBeenCalledTimes(1);
      expect(createUserSpy).toHaveBeenCalledTimes(1);
      expect(logSpy).not.toHaveBeenCalled();
      expect(actual).toBe(mockUser);
    });
  });

  describe('getAll', () => {
    let findUsersPaginatedSpy: jest.SpyInstance;

    const expectedRequestDto: GetUsersDto = {
      paginatedRequest: {
        limit: 10,
        offset: 0,
      },
      isActive: true,
    };

    beforeEach((done) => {
      findUsersPaginatedSpy = jest.spyOn(repository, 'findUsersPaginated');
      done();
    });

    test('should throw NotFoundException when no existing users not found', async () => {
      findUsersPaginatedSpy.mockImplementation(() =>
        Promise.resolve({
          users: [],
          totalCount: 0,
        }),
      );

      try {
        await service.getUsersPaginated(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe('No users found.');
      }

      expect(repository.findUsersPaginated).toHaveBeenCalledWith(
        expectedRequestDto.paginatedRequest,
        expectedRequestDto.isActive,
      );
      expect(findUsersPaginatedSpy).toHaveBeenCalledTimes(1);
    });

    test('should return user', async () => {
      findUsersPaginatedSpy.mockImplementation(() =>
        Promise.resolve({ users: [mockUser], totalCount: 1 }),
      );

      const actual = await service.getUsersPaginated(expectedRequestDto);

      expect(repository.findUsersPaginated).toHaveBeenCalledWith(
        expectedRequestDto.paginatedRequest,
        expectedRequestDto.isActive,
      );

      expect(findUsersPaginatedSpy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual({ users: [mockUser], totalCount: 1 });
    });
  });

  describe('findByEmail', () => {
    let findOneSpy: jest.SpyInstance;

    const expectedRequestDto: FindUserByEmailDto = {
      email: mockUser.email,
    };

    beforeEach((done) => {
      findOneSpy = jest.spyOn(repository, 'findOne');
      done();
    });

    test('should throw NotFoundException user with email not found', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve());

      try {
        await service.findByEmail(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe(
          `User with email ${mockUser.email} could not be found`,
        );
      }

      expect(repository.findOne).toHaveBeenCalledWith({
        email: expectedRequestDto.email,
      });
      expect(findOneSpy).toHaveBeenCalledTimes(1);
    });

    test('should return user', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve(mockUser));
      const actual = await service.findByEmail(expectedRequestDto);

      expect(repository.findOne).toHaveBeenCalledWith({
        email: expectedRequestDto.email,
      });

      expect(findOneSpy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(mockUser);
    });
  });

  describe('findByUsername', () => {
    let findOneSpy: jest.SpyInstance;

    const expectedRequestDto: FindUserByUsernameDto = {
      username: mockUser.username,
    };

    beforeEach((done) => {
      findOneSpy = jest.spyOn(repository, 'findOne');
      done();
    });

    test('should throw NotFoundException user with username not found', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve());

      try {
        await service.findByUsername(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe(
          `User with username ${mockUser.username} could not be found`,
        );
      }

      expect(repository.findOne).toHaveBeenCalledWith({
        username: expectedRequestDto.username,
      });
      expect(findOneSpy).toHaveBeenCalledTimes(1);
    });

    test('should return user', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve(mockUser));
      const actual = await service.findByUsername(expectedRequestDto);

      expect(repository.findOne).toHaveBeenCalledWith({
        username: expectedRequestDto.username,
      });

      expect(findOneSpy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(mockUser);
    });
  });

  describe('delete', () => {
    let findOneSpy: jest.SpyInstance;
    let saveSpy: jest.SpyInstance;
    let logSpy: jest.SpyInstance;

    const expectedRequestDto: FindUserByEmailDto = {
      email: mockUser.email,
    };

    beforeEach((done) => {
      findOneSpy = jest.spyOn(repository, 'findOne');
      saveSpy = jest.spyOn(repository, 'save');
      logSpy = jest.spyOn(logger, 'log');
      done();
    });

    test('should throw NotFoundException when user with email not found', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve());

      try {
        await service.delete(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe(
          `User with email ${mockUser.email} could not be found`,
        );
      }

      expect(repository.findOne).toHaveBeenCalledWith({
        email: expectedRequestDto.email,
      });

      expect(logger.log).toHaveBeenCalledWith(
        `delete: User with email ${mockUser.email} not found.`,
        UserService.name,
      );

      expect(findOneSpy).toHaveBeenCalledTimes(1);
      expect(logSpy).toHaveBeenCalledTimes(1);
    });

    test('should call delete', async () => {
      const userToDelete: User = {
        ...mockUser,
        isActive: false,
      };

      findOneSpy.mockImplementation(() => Promise.resolve(userToDelete));
      saveSpy.mockImplementation(() => Promise.resolve());

      await service.delete(expectedRequestDto);

      expect(findOneSpy).toHaveBeenCalledTimes(1);
      expect(saveSpy).toHaveBeenCalledTimes(1);

      expect(repository.findOne).toHaveBeenCalledWith({
        email: expectedRequestDto.email,
      });
      expect(repository.save).toHaveBeenCalledWith(userToDelete);

      expect(logSpy).not.toHaveBeenCalled();
    });
  });

  describe('updateRoles', () => {
    let findOneSpy: jest.SpyInstance;
    let saveSpy: jest.SpyInstance;
    let logSpy: jest.SpyInstance;

    const expectedRequestDto: UpdateRolesDto = {
      email: mockUser.email,
      roles: [Role.Admin, Role.Member],
    };

    beforeEach((done) => {
      findOneSpy = jest.spyOn(repository, 'findOne');
      saveSpy = jest.spyOn(repository, 'save');
      logSpy = jest.spyOn(logger, 'log');
      done();
    });

    test('should throw NotFoundException when user with email not found', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve(undefined));

      try {
        await service.updateRoles(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe(
          `User with email ${expectedRequestDto.email} could not be found.`,
        );
      }

      expect(repository.findOne).toHaveBeenCalledWith({
        email: expectedRequestDto.email,
      });

      expect(logger.log).toHaveBeenCalledWith(
        `updateRoles: User with email ${expectedRequestDto.email} not found.`,
        UserService.name,
      );

      expect(findOneSpy).toHaveBeenCalledTimes(1);
      expect(logSpy).toHaveBeenCalledTimes(1);
      expect(saveSpy).not.toHaveBeenCalled();
    });

    test('should update user roles', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve(mockUser));

      await service.updateRoles(expectedRequestDto);

      expect(repository.findOne).toHaveBeenCalledWith({
        email: expectedRequestDto.email,
      });

      expect(repository.save).toHaveBeenCalledWith({
        ...mockUser,
        roles: expectedRequestDto.roles,
      });

      expect(findOneSpy).toHaveBeenCalledTimes(1);
      expect(saveSpy).toHaveBeenCalledTimes(1);
      expect(logSpy).not.toHaveBeenCalled();
    });
  });

  describe('updateUsername', () => {
    let findOneSpy: jest.SpyInstance;
    let saveSpy: jest.SpyInstance;
    let logSpy: jest.SpyInstance;
    let errorSpy: jest.SpyInstance;

    const expectedRequestDto: UpdateUsernameDto = {
      username: `${mockUser.username}_2`,
    };

    beforeEach((done) => {
      findOneSpy = jest.spyOn(repository, 'findOne');
      saveSpy = jest.spyOn(repository, 'save');
      logSpy = jest.spyOn(logger, 'log');
      errorSpy = jest.spyOn(logger, 'error');
      done();
    });

    test('should throw ConflictException when another existing user with username found', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve([mockUser]));

      try {
        await service.updateUsername(expectedRequestDto, mockUser);
      } catch (error) {
        expect(error).toBeInstanceOf(ConflictException);
        expect(error.message).toBe(
          `user with username ${expectedRequestDto.username} already exists.`,
        );
      }

      expect(logger.log).toHaveBeenCalledWith(
        `updateUsername: User with username ${expectedRequestDto.username} already exists.`,
        UserService.name,
      );

      expect(findOneSpy).toHaveBeenCalledTimes(1);
      expect(logSpy).toHaveBeenCalledTimes(1);
      expect(saveSpy).not.toHaveBeenCalled();
    });

    test('should throw NotFoundException when user with email not found', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve());
      findOneSpy.mockImplementation(() => Promise.resolve());

      try {
        await service.updateUsername(expectedRequestDto, mockUser);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe(
          `User with email ${mockUser.email} could not be found`,
        );
      }

      expect(repository.findOne).toHaveBeenCalledWith({
        email: mockUser.email,
      });

      expect(logger.error).toHaveBeenCalledWith(
        `updateUsername: User with email ${mockUser.email} not found.`,
        UserService.name,
      );

      expect(findOneSpy).toHaveBeenCalledTimes(2);
      expect(errorSpy).toHaveBeenCalledTimes(1);
      expect(logSpy).not.toHaveBeenCalled();
      expect(saveSpy).not.toHaveBeenCalled();
    });

    test('should return user', async () => {
      const expectedResponse = {
        ...mockUser,
        username: expectedRequestDto.username,
      };

      findOneSpy
        .mockReturnValueOnce(undefined)
        .mockReturnValueOnce({ ...mockUser, id: '1' });
      saveSpy.mockImplementation(() => Promise.resolve(expectedResponse));

      const actual = await service.updateUsername(expectedRequestDto, mockUser);

      expect(findOneSpy).toHaveBeenCalledTimes(2);
      expect(saveSpy).toHaveBeenCalledTimes(1);
      expect(logSpy).not.toHaveBeenCalled();
      expect(errorSpy).not.toHaveBeenCalled();
      expect(actual).toEqual(expectedResponse);
    });
  });
});
