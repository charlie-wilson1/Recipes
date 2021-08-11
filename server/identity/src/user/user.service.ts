import {
  ConflictException,
  Injectable,
  NotFoundException,
} from '@nestjs/common';
import { CreateUserDto } from './dtos/createUser.dto';
import { UpdateUsernameDto } from './dtos/updateUsername.dto';
import { UpdateRolesDto } from './dtos/updateRoles.dto';
import { UserRepository } from './user.repository';
import { GetUsersDto } from './dtos/getUsers.dto';
import { InjectRepository } from '@nestjs/typeorm';
import { User } from './entities/user.entity';
import { FindUserByEmailDto } from './dtos/findUserByEmail.dto';
import { FindUserByUsernameDto } from './dtos/FindUserByUsername.dto';
import { Not } from 'typeorm';

@Injectable()
export class UserService {
  constructor(
    @InjectRepository(UserRepository)
    private userRepository: UserRepository,
  ) {}

  async create(createDto: CreateUserDto): Promise<User> {
    const existingUser = await this.userRepository.findOne({
      email: createDto.email,
    });

    if (existingUser) {
      throw new ConflictException(
        `user with email ${createDto.email} already exists.`,
      );
    }

    return await this.userRepository.createUser(createDto.email);
  }

  async getUsersPaginated(
    getAllDto: GetUsersDto,
  ): Promise<{ users: User[]; totalCount: number }> {
    const { paginatedRequest, isActive = true } = getAllDto;

    const result = await this.userRepository.findUsersPaginated(
      paginatedRequest,
      isActive,
    );

    // if ((result?.totalCount ?? 0) === 0) {
    //   throw new NotFoundException('No users found.');
    // }

    return result;
  }

  async findByEmail(findDto: FindUserByEmailDto): Promise<User> {
    const result = this.userRepository.findOne({ email: findDto.email });

    if (!result) {
      throw new NotFoundException(
        `User with email ${findDto.email} could not be found`,
      );
    }

    return result;
  }

  async findByUsername(findDto: FindUserByUsernameDto): Promise<User> {
    const result = await this.userRepository.findOne({
      username: findDto.username,
    });

    if (!result) {
      throw new NotFoundException(
        `User with username ${findDto.username} could not be found`,
      );
    }

    return result;
  }

  async delete(deleteDto: FindUserByEmailDto): Promise<void> {
    const email = decodeURI(deleteDto.email);
    const userToDelete = await this.userRepository.findOne({ email });

    if (!userToDelete) {
      throw new NotFoundException(
        `User with email ${deleteDto.email} could not be found`,
      );
    }

    userToDelete.isActive = false;
    await this.userRepository.save(userToDelete);
  }

  async updateRoles(updateDto: UpdateRolesDto): Promise<User> {
    const user = await this.userRepository.findOne({
      email: updateDto.email,
    });

    if (!user) {
      throw new NotFoundException(
        `User with email ${updateDto.email} could not be found`,
      );
    }

    user.roles = updateDto.roles;
    return await this.userRepository.save(user);
  }

  async updateUsername(
    updateDto: UpdateUsernameDto,
    currentUser: User,
  ): Promise<User> {
    const existingUserWithUsername = await this.userRepository.findOne({
      username: updateDto.username,
      email: Not(currentUser.email),
    });

    if (existingUserWithUsername) {
      throw new ConflictException(
        `user with username ${updateDto.username} already exists.`,
      );
    }

    const userToUpdate = await this.userRepository.findOne({
      email: currentUser.email,
    });

    if (!userToUpdate) {
      throw new NotFoundException(
        `User with email ${currentUser.email} could not be found`,
      );
    }

    userToUpdate.username = updateDto.username;
    return await this.userRepository.save(userToUpdate);
  }
}
