import { IsEmail } from 'class-validator';

export class CreateProfileDto {
  @IsEmail()
  readonly email: string;
}
