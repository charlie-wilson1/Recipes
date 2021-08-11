import { Injectable } from '@nestjs/common';
import { Magic, MagicUserMetadata } from '@magic-sdk/admin';
import { JwtService } from '@nestjs/jwt';
import { UserRepository } from '../user/user.repository';
import { InjectRepository } from '@nestjs/typeorm';
import { User } from '../user/entities/user.entity';

@Injectable()
export class AuthenticationService {
  constructor(
    @InjectRepository(User)
    private readonly userRepository: UserRepository,
    private readonly jwtService: JwtService,
  ) {}
  private readonly magic = new Magic(process.env.MAGIC_KEY);

  async authenticateDidToken(didToken: string): Promise<MagicUserMetadata> {
    this.magic.token.validate(didToken);
    return await this.magic.users.getMetadataByToken(didToken);
  }

  async getUser(email: string): Promise<User> {
    return await this.userRepository.findOne({ email });
  }

  createJwtFromUser(user: User): string {
    const payload = {
      roles: user.roles,
      name: user.username,
      email: user.email,
      nameid: user.email,
    };

    return this.jwtService.sign(payload);
  }

  async logout(didToken: string): Promise<void> {
    this.magic.users.logoutByToken(didToken);
  }
}
