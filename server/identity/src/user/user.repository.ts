import { PaginationQueryDto } from 'src/models/paginationQueryDto';
import { EntityRepository, Not, Repository } from 'typeorm';
import { Role } from '../models/roles';
import { User } from './entities/user.entity';

@EntityRepository(User)
export class UserRepository extends Repository<User> {
  async createUser(email: string): Promise<User> {
    const createdUser = this.create({
      email,
      roles: [Role.Member],
      isActive: true,
    });

    return await this.save(createdUser);
  }

  async findUsersPaginated(
    paginationQuery: PaginationQueryDto,
    isActive,
  ): Promise<{ users: User[]; totalCount: number }> {
    let limit = 10;
    let offset = 0;

    if (paginationQuery) {
      limit = paginationQuery.limit;
      offset = paginationQuery.offset;
    }

    const query = this.createQueryBuilder();

    query.where({ isActive }).skip(offset).limit(limit);

    const users = await query.getMany();
    const totalCount = await query.getCount();

    return { users, totalCount };
  }
}
