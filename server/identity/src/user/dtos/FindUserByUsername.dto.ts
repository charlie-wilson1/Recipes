import { IsNotEmpty } from 'class-validator';

export class FindUserByUsernameDto {
  @IsNotEmpty()
  readonly username: string;
}
