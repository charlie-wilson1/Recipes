import {
  ConflictException,
  Injectable,
  NotFoundException,
} from '@nestjs/common';
import { PaginationQueryDto } from '../models/paginationQueryDto';
import { CreateProfileDto } from './models/createProfileDto';
import {
  FindProfileByEmailDto,
  FindProfileByUsernameDto,
} from './models/findProfileDtos';
import { Profile } from './models/profile.schema';
import { UpdateUsernameDto } from './models/updateUsernameDto';
import { UpdateRolesDto } from './models/updateRolesDto';
import { ProfileRepository } from './profile.repository';

@Injectable()
export class ProfileService {
  constructor(private readonly profileRepository: ProfileRepository) {}

  async create(profile: CreateProfileDto): Promise<Profile> {
    const existingUser = await this.profileRepository.findByEmail(
      profile.email,
    );

    if (existingUser) {
      throw new ConflictException(
        `profile with email ${profile.email} already exists.`,
      );
    }

    return await this.profileRepository.create(profile.email);
  }

  async getAll(
    paginationQuery: PaginationQueryDto,
    isActive: boolean,
  ): Promise<Profile[]> {
    const result = await this.profileRepository.getAll(
      paginationQuery,
      isActive,
    );

    if ((result || []).length === 0) {
      throw new NotFoundException('No profiles found.');
    }

    return result;
  }

  async findByEmail(profile: FindProfileByEmailDto): Promise<Profile> {
    const result = this.profileRepository.findByEmail(profile.email);

    if (!result) {
      throw new NotFoundException(
        `User with email ${profile.email} could not be found`,
      );
    }

    return result;
  }

  async findByUsername(profile: FindProfileByUsernameDto): Promise<Profile> {
    const result = this.profileRepository.findByUsername(profile.username);

    if (!result) {
      throw new NotFoundException(
        `User with email ${profile.username} could not be found`,
      );
    }

    return result;
  }

  async delete(profile: FindProfileByEmailDto): Promise<void> {
    const profileToDelete = await this.profileRepository.findByEmail(
      profile.email,
    );

    if (!profileToDelete) {
      throw new NotFoundException(
        `User with email ${profile.email} could not be found`,
      );
    }

    await this.profileRepository.delete(profileToDelete.id);
  }

  async updateRoles(profile: UpdateRolesDto): Promise<Profile> {
    const profileToUpdate = await this.profileRepository.findByEmail(
      profile.email,
    );

    if (!profileToUpdate) {
      throw new NotFoundException(
        `User with email ${profile.email} could not be found`,
      );
    }

    profileToUpdate.roles = profile.roles;
    return this.profileRepository.update(profileToUpdate.id, profileToUpdate);
  }

  async updateProfileUsername(profileDto: UpdateUsernameDto): Promise<Profile> {
    const existingProfilesWithUsername =
      await this.profileRepository.findByUsernameNotEmail(
        profileDto.username,
        profileDto.email,
      );

    if (existingProfilesWithUsername.length > 0) {
      throw new ConflictException(
        `profile with username ${profileDto.username} already exists.`,
      );
    }

    const profileToUpdate = await this.profileRepository.findByEmail(
      profileDto.email,
    );

    if (!profileToUpdate) {
      throw new NotFoundException(
        `User with email ${profileDto.email} could not be found`,
      );
    }

    profileToUpdate.username = profileDto.username;
    return await this.profileRepository.update(
      profileToUpdate.id,
      profileToUpdate,
    );
  }
}
