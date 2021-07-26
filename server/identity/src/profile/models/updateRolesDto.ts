import {
  ArrayContains,
  ArrayNotEmpty,
  IsArray,
  IsEmail,
} from 'class-validator';
import { Role } from '../../models/roles';

export class UpdateRolesDto {
  @IsEmail()
  email: string;

  @IsArray()
  @ArrayNotEmpty()
  @ArrayContains(['Member'])
  roles: Role[];
}
