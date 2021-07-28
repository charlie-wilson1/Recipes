import {
  Body,
  Controller,
  Delete,
  Get,
  HttpCode,
  Post,
  Put,
  Query,
  UseGuards,
} from '@nestjs/common';
import { Roles } from '../decorators/roles.decorator';
import { CurrentUserGuard } from '../guards/currentUser.guard';
import { JwtAuthGuard } from '../guards/jwt.guard';
import { RolesGuard } from '../guards/roles.guard';
import { Role } from '../models/roles';
import { CreateProfileDto } from './models/createProfileDto';
import {
  FindProfileByEmailDto,
  FindProfileByUsernameDto,
} from './models/findProfileDtos';
import { Profile } from './models/profile.schema';
import { UpdateUsernameDto } from './models/updateUsernameDto';
import { UpdateRolesDto } from './models/updateRolesDto';
import { ProfileService } from './profile.service';
import { StatusCodes } from 'http-status-codes';
import { GetAllDto } from './models/getAllDto';

@UseGuards(JwtAuthGuard)
@Controller('profile')
export class ProfileController {
  constructor(private profileService: ProfileService) {}

  @UseGuards(RolesGuard)
  @Roles(Role.Admin)
  @HttpCode(StatusCodes.CREATED)
  @Post()
  async create(@Body() profile: CreateProfileDto): Promise<Profile> {
    return await this.profileService.create(profile);
  }

  @UseGuards(RolesGuard)
  @Roles(Role.Admin)
  @Get()
  async getAll(@Query() getAllDto: GetAllDto): Promise<Profile[]> {
    return await this.profileService.getAll(getAllDto);
  }

  @UseGuards(RolesGuard)
  @Roles(Role.Admin)
  @Get('email')
  async getByEmail(@Query() dto: FindProfileByEmailDto): Promise<Profile> {
    return await this.profileService.findByEmail(dto);
  }

  @Get('username')
  async getByUsername(
    @Query() dto: FindProfileByUsernameDto,
  ): Promise<Profile> {
    return await this.profileService.findByUsername(dto);
  }

  @UseGuards(RolesGuard)
  @Roles(Role.Admin)
  @HttpCode(StatusCodes.NO_CONTENT)
  @Delete()
  async delete(@Body() dto: FindProfileByEmailDto): Promise<void> {
    await this.profileService.delete(dto);
  }

  @UseGuards(RolesGuard)
  @Roles(Role.Admin)
  @Put('roles')
  async updateRoles(@Body() dto: UpdateRolesDto): Promise<Profile> {
    return await this.profileService.updateRoles(dto);
  }

  @UseGuards(CurrentUserGuard)
  @Put('username')
  async updateUsername(@Body() dto: UpdateUsernameDto): Promise<Profile> {
    return await this.profileService.updateProfileUsername(dto);
  }
}
