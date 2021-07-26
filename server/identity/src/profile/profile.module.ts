import { Module } from '@nestjs/common';
import { ProfileService } from './profile.service';
import { ProfileController } from './profile.controller';
import { MongooseModule } from '@nestjs/mongoose';
import { Profile, ProfileSchema } from './models/profile.schema';
import { ProfileRepository } from './profile.repository';

@Module({
  controllers: [ProfileController],
  imports: [
    MongooseModule.forFeature([
      {
        name: Profile.name,
        schema: ProfileSchema,
      },
    ]),
  ],
  providers: [ProfileService, ProfileRepository],
  exports: [ProfileRepository],
})
export class ProfileModule {}
