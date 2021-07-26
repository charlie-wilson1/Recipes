import { IsEmail, IsNotEmpty } from 'class-validator';

export class FindProfileByEmailDto {
  @IsEmail()
  readonly email: string;
}

export class FindProfileByUsernameDto {
  @IsNotEmpty()
  readonly username: string;
}
