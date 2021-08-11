import { IsEmail } from 'class-validator';

export class FindUserByEmailDto {
  @IsEmail()
  readonly email: string;
}
