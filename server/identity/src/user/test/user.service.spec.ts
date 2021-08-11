import { Test, TestingModule } from '@nestjs/testing';
import { UserService } from '../user.service';
import { UserRepository } from '../user.repository';
import { ConflictException, NotFoundException } from '@nestjs/common';
import { GetUsersDto } from '../dtos/getUsers.dto';
import { FindUserByEmailDto } from '../dtos/findUserByEmail.dto';
import { FindUserByUsernameDto } from '../dtos/FindUserByUsername.dto';
import { UpdateUsernameDto } from '../dtos/updateUsername.dto';
import { User } from '../entities/user.entity';
import { Role } from '../../models/roles';

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
  id: '1',
  username: 'username',
  roles: [Role.Member],
  email: 'test@test.com',
  isActive: true,
};

describe('UserService', () => {
  let service: UserService;
  let repository: UserRepository;

  beforeEach(async (done) => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        UserService,
        { provide: UserRepository, useFactory: mockUserRepository },
      ],
    }).compile();

    service = module.get<UserService>(UserService);
    repository = module.get<UserRepository>(UserRepository);
    jest.resetAllMocks();
    done();
  });

  test('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('create', () => {
    let findOneSpy: jest.SpyInstance;
    let createUserSpy: jest.SpyInstance;

    beforeEach((done) => {
      findOneSpy = jest.spyOn(repository, 'findOne');
      createUserSpy = jest.spyOn(repository, 'createUser');
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

      expect(findOneSpy).toHaveBeenCalledTimes(1);
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

    const expectedRequestDto: FindUserByEmailDto = {
      email: mockUser.email,
    };

    beforeEach((done) => {
      findOneSpy = jest.spyOn(repository, 'findOne');
      saveSpy = jest.spyOn(repository, 'save');
      done();
    });

    test('should throw NotFoundException when user with email not found', async () => {
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
    });
  });

  describe('updateRoles', () => {
    let findByUsernameNotEmailSpy: jest.SpyInstance;
    let findOneSpy: jest.SpyInstance;
    let saveSpy: jest.SpyInstance;

    const expectedRequestDto: UpdateUsernameDto = {
      email: mockUser.email,
      username: `${mockUser.username}_2`,
    };

    beforeEach((done) => {
      findByUsernameNotEmailSpy = jest.spyOn(
        repository,
        'findByUsernameNotEmail',
      );
      findOneSpy = jest.spyOn(repository, 'findOne');
      saveSpy = jest.spyOn(repository, 'save');
      done();
    });

    test('should throw ConflictException when another existing user with username found', async () => {
      findByUsernameNotEmailSpy.mockImplementation(() =>
        Promise.resolve([mockUser]),
      );

      try {
        await service.updateUsername(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(ConflictException);
        expect(error.message).toBe(
          `user with username ${expectedRequestDto.username} already exists.`,
        );
      }

      expect(repository.findByUsernameNotEmail).toHaveBeenCalledWith(
        expectedRequestDto.username,
        expectedRequestDto.email,
      );

      expect(findByUsernameNotEmailSpy).toHaveBeenCalledTimes(1);
      expect(findOneSpy).not.toHaveBeenCalled();
      expect(saveSpy).not.toHaveBeenCalled();
    });

    test('should throw NotFoundException when user with email not found', async () => {
      findByUsernameNotEmailSpy.mockImplementation(() => Promise.resolve());
      findOneSpy.mockImplementation(() => Promise.resolve());

      try {
        await service.updateUsername(expectedRequestDto);
      } catch (error) {
        expect(error).toBeInstanceOf(NotFoundException);
        expect(error.message).toBe(
          `User with email ${mockUser.email} could not be found`,
        );
      }

      expect(repository.findByUsernameNotEmail).toHaveBeenCalledWith(
        expectedRequestDto.username,
        expectedRequestDto.email,
      );
      expect(repository.findOne).toHaveBeenCalledWith({
        email: expectedRequestDto.email,
      });

      expect(findByUsernameNotEmailSpy).toHaveBeenCalledTimes(1);
      expect(findOneSpy).toHaveBeenCalledTimes(1);
      expect(saveSpy).not.toHaveBeenCalled();
    });

    test('should return user', async () => {
      const expectedResponse = {
        ...mockUser,
        username: expectedRequestDto.username,
      };

      findByUsernameNotEmailSpy.mockImplementation(() => Promise.resolve());
      findOneSpy.mockImplementation(() =>
        Promise.resolve({ ...mockUser, id: '1' }),
      );
      saveSpy.mockImplementation(() => Promise.resolve(expectedResponse));

      const actual = await service.updateUsername(expectedRequestDto);

      expect(repository.findByUsernameNotEmail).toHaveBeenCalledWith(
        expectedRequestDto.username,
        expectedRequestDto.email,
      );
      expect(repository.findOne).toHaveBeenCalledWith({
        email: expectedRequestDto.email,
      });

      expect(repository.save).toHaveBeenCalledWith(expectedResponse);

      expect(findByUsernameNotEmailSpy).toHaveBeenCalledTimes(1);
      expect(findOneSpy).toHaveBeenCalledTimes(1);
      expect(saveSpy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(expectedResponse);
    });
  });
});
