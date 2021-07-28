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

  private async getUser(email: string): Promise<Profile> {
    const user = await this.profileRepository.findByEmail(email);

    if (!user?.isActive) {
      throw new UnauthorizedException(
        `user with email ${email} is not authorized to use this application. Please contact an administrator to be invited to use the application.`,
      );
    }

    return user;
  }

  async createJwtFromMagicMetadata(metadata: MagicUserMetadata): Promise<any> {
    const user = await this.getUser(metadata.email);

    const payload = {
      publicAddress: metadata.publicAddress,
      nameid: metadata.issuer,
      roles: user.roles,
      name: user.username,
    };

    return {
      access_token: await this.jwtService.signAsync(payload),
    };
  }

  async logout(didToken: string): Promise<void> {
    this.magic.users.logoutByToken(didToken);
  }
}
