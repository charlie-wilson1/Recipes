import { IsOptional } from 'class-validator';
import { PaginationQueryDto } from '../../models/paginationQueryDto';

export class GetAllDto {
  @IsOptional()
  readonly paginatedRequest: PaginationQueryDto;

  @IsOptional()
  isActive: boolean;
}
