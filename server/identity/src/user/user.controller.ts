import {
  Body,
  Controller,
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
import { CreateUserDto } from './dtos/createUser.dto';
import { FindUserByEmailDto } from './dtos/findUserByEmail.dto';
import { FindUserByUsernameDto } from './dtos/FindUserByUsername.dto';
import { UpdateUsernameDto } from './dtos/updateUsername.dto';
import { UpdateRolesDto } from './dtos/updateRoles.dto';
import { UserService } from './user.service';
import { StatusCodes } from 'http-status-codes';
import { GetUsersDto } from './dtos/getUsers.dto';
import { User } from './entities/user.entity';
import { GetUser } from 'src/decorators/get-user.decorator';

@UseGuards(JwtAuthGuard)
@Controller('user')
export class UserController {
  constructor(private userService: UserService) {}

  // @UseGuards(RolesGuard)
  // @Roles(Role.Admin)
  @HttpCode(StatusCodes.CREATED)
  @Post()
  async create(@Body() user: CreateUserDto): Promise<User> {
    return await this.userService.create(user);
  }

  @UseGuards(RolesGuard)
  @Roles(Role.Admin)
  @Get()
  async get(
    @Query() limit: number,
    @Query() offset: number,
  ): Promise<{ users: User[]; totalCount: number }> {
    const getUsersDto = new GetUsersDto();
    getUsersDto.isActive = true;
    getUsersDto.paginatedRequest = {
      limit,
      offset,
    };
    return await this.userService.getUsersPaginated(getUsersDto);
  }

  @UseGuards(RolesGuard)
  @Roles(Role.Admin)
  @Get('email')
  async getByEmail(@Query() dto: FindUserByEmailDto): Promise<User> {
    return await this.userService.findByEmail(dto);
  }

  @Get('username')
  async getByUsername(@Query() dto: FindUserByUsernameDto): Promise<User> {
    return await this.userService.findByUsername(dto);
  }

  @UseGuards(RolesGuard)
  @Roles(Role.Admin)
  @HttpCode(StatusCodes.NO_CONTENT)
  @Put('delete')
  async delete(@Body() dto: FindUserByEmailDto): Promise<void> {
    await this.userService.delete(dto);
  }

  @UseGuards(RolesGuard)
  @Roles(Role.Admin)
  @Put('roles')
  async updateRoles(@Body() dto: UpdateRolesDto): Promise<User> {
    return await this.userService.updateRoles(dto);
  }

  @UseGuards(CurrentUserGuard)
  @Put('username')
  async updateUsername(
    @Body() dto: UpdateUsernameDto,
    @GetUser() user: User,
  ): Promise<User> {
    return await this.userService.updateUsername(dto, user);
  }
}
