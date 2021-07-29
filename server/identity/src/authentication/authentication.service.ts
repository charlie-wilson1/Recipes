import { Injectable, UnauthorizedException } from '@nestjs/common';
import { Magic, MagicUserMetadata } from '@magic-sdk/admin';
import { JwtService } from '@nestjs/jwt';
import { ProfileRepository } from '../profile/profile.repository';
import { Profile } from 'src/profile/models/profile.schema';

@Injectable()
export class AuthenticationService {
  constructor(
    private readonly jwtService: JwtService,
    private readonly profileRepository: ProfileRepository,
  ) {}
  private readonly magic = new Magic(process.env.MAGIC_KEY);

  async authenticateDidToken(didToken: string): Promise<MagicUserMetadata> {
    this.magic.token.validate(didToken);
    return await this.magic.users.getMetadataByToken(didToken);
  }

  async getProfile(email: string): Promise<Profile> {
    return await this.profileRepository.findByEmail(email);
  }

  createJwtFromProfile(profile: Profile): string {
    const payload = {
      roles: profile.roles,
      name: profile.username,
      nameid: profile.email,
    };

    return this.jwtService.sign(payload);
  }

  async logout(didToken: string): Promise<void> {
    this.magic.users.logoutByToken(didToken);
  }
}
