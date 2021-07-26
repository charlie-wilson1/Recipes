import { Injectable } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';
import { PaginationQueryDto } from '../models/paginationQueryDto';
import { Role } from '../models/roles';
import { Profile, ProfileDocument } from './models/profile.schema';

@Injectable()
export class ProfileRepository {
  constructor(
    @InjectModel(Profile.name)
    private readonly profileModel: Model<ProfileDocument>,
  ) {}

  async create(email: string): Promise<ProfileDocument> {
    const createdUser = new this.profileModel({
      email,
      roles: [Role.Member],
      isActive: true,
    });

    return await createdUser.save();
  }

  async findByEmail(email: string): Promise<ProfileDocument> {
    return await this.profileModel.findOne({ email }).exec();
  }

  async findByUsername(username: string): Promise<ProfileDocument> {
    return await this.profileModel.findOne({ username }).exec();
  }

  async getAll(
    paginationQuery: PaginationQueryDto,
    isActive,
  ): Promise<ProfileDocument[]> {
    const { limit = 20, offset = 0 } = paginationQuery;

    const result = await this.profileModel
      .find({ isActive })
      .skip(offset)
      .limit(limit)
      .exec();

    return result;
  }

  async delete(profileId: string): Promise<void> {
    await this.profileModel
      .findByIdAndUpdate(profileId, {
        isActive: false,
      })
      .exec();
  }

  async update(id: string, profileToUpdate: Profile): Promise<ProfileDocument> {
    await this.profileModel.findByIdAndUpdate(id, profileToUpdate);
    return this.profileModel.findById(id).exec();
  }

  async findByUsernameNotEmail(username: string, email: string) {
    return await this.profileModel
      .find({ username })
      .where('email')
      .ne(email)
      .exec();
  }
}
