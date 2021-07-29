import {
  ConflictException,
  Injectable,
  NotFoundException,
} from '@nestjs/common';
import { CreateProfileDto } from './models/createProfileDto';
import {
  FindProfileByEmailDto,
  FindProfileByUsernameDto,
} from './models/findProfileDtos';
import { Profile } from './models/profile.schema';
import { UpdateUsernameDto } from './models/updateUsernameDto';
import { UpdateRolesDto } from './models/updateRolesDto';
import { ProfileRepository } from './profile.repository';
import { GetAllDto } from './models/getAllDto';

@Injectable()
export class ProfileService {
  constructor(private readonly profileRepository: ProfileRepository) {}

  async create(createDto: CreateProfileDto): Promise<Profile> {
    const existingUser = await this.profileRepository.findByEmail(
      createDto.email,
    );

    if (existingUser) {
      throw new ConflictException(
        `profile with email ${createDto.email} already exists.`,
      );
    }

    return await this.profileRepository.create(createDto.email);
  }

  async getAll(getAllDto: GetAllDto): Promise<Profile[]> {
    const { paginatedRequest, isActive = true } = getAllDto;

    const result = await this.profileRepository.getAll(
      paginatedRequest,
      isActive,
    );

    if ((result ?? []).length === 0) {
      throw new NotFoundException('No profiles found.');
    }

    return result;
  }

  async findByEmail(findDto: FindProfileByEmailDto): Promise<Profile> {
    const result = this.profileRepository.findByEmail(findDto.email);

    if (!result) {
      throw new NotFoundException(
        `User with email ${findDto.email} could not be found`,
      );
    }

    return result;
  }

  async findByUsername(findDto: FindProfileByUsernameDto): Promise<Profile> {
    const result = this.profileRepository.findByUsername(findDto.username);

    if (!result) {
      throw new NotFoundException(
        `User with username ${findDto.username} could not be found`,
      );
    }

    return result;
  }

  async delete(deleteDto: FindProfileByEmailDto): Promise<void> {
    const email = decodeURI(deleteDto.email);
    const profileToDelete = await this.profileRepository.findByEmail(email);

    if (!profileToDelete) {
      throw new NotFoundException(
        `User with email ${deleteDto.email} could not be found`,
      );
    }

    await this.profileRepository.delete(profileToDelete.id);
  }

  async updateRoles(updateDto: UpdateRolesDto): Promise<Profile> {
    const profileToUpdate = await this.profileRepository.findByEmail(
      updateDto.email,
    );

    if (!profileToUpdate) {
      throw new NotFoundException(
        `User with email ${updateDto.email} could not be found`,
      );
    }

    profileToUpdate.roles = updateDto.roles;
    return this.profileRepository.update(profileToUpdate.id, profileToUpdate);
  }

  async updateProfileUsername(updateDto: UpdateUsernameDto): Promise<Profile> {
    const existingProfilesWithUsername =
      await this.profileRepository.findByUsernameNotEmail(
        updateDto.username,
        updateDto.email,
      );

    if ((existingProfilesWithUsername ?? []).length > 0) {
      throw new ConflictException(
        `profile with username ${updateDto.username} already exists.`,
      );
    }

    const profileToUpdate = await this.profileRepository.findByEmail(
      updateDto.email,
    );

    if (!profileToUpdate) {
      throw new NotFoundException(
        `User with email ${updateDto.email} could not be found`,
      );
    }

    profileToUpdate.username = updateDto.username;
    return await this.profileRepository.update(
      profileToUpdate.id,
      profileToUpdate,
    );
  }
}
