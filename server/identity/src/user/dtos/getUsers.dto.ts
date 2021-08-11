import { IsOptional } from 'class-validator';
import { PaginationQueryDto } from '../../models/paginationQueryDto';

export class GetUsersDto {
  @IsOptional()
  paginatedRequest: PaginationQueryDto;

  @IsOptional()
  isActive: boolean;
}
