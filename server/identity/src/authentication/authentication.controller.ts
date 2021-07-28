import { Body, Controller, Post, UnauthorizedException } from '@nestjs/common';
import { AuthenticationService } from './authentication.service';
import { AuthRequestDto } from './models/loginRequestDto';

@Controller('authorization')
export class AuthenticationController {
  constructor(private readonly authenticationService: AuthenticationService) {}

  @Post('login')
  async authenticate(@Body() loginRequestDto: AuthRequestDto): Promise<any> {
    console.log('gets here');
    const metadata = await this.authenticationService.authenticateDidToken(
      loginRequestDto.didToken,
    );

    if (!metadata) {
      this.authenticationService.logout(loginRequestDto.didToken);
      throw new UnauthorizedException();
    }

    return await this.authenticationService.createJwtFromMagicMetadata(
      metadata,
    );
  }

  @Post('logout')
  async logout(@Body() logoutRequestDto: AuthRequestDto): Promise<void> {
    await this.authenticationService.logout(logoutRequestDto.didToken);
  }
}
