import { Body, Controller, Post } from '@nestjs/common';
import { AuthenticationService } from './authentication.service';
import { AuthRequestDto } from './models/loginRequestDto';

@Controller('identity')
export class AuthenticationController {
  constructor(private readonly authenticationService: AuthenticationService) {}

  @Post('login')
  async authenticate(@Body() loginRequestDto: AuthRequestDto): Promise<any> {
    const metadata = await this.authenticationService.authenticateDidToken(
      loginRequestDto.didToken,
    );
    return await this.authenticationService.createJwtFromMagicMetadata(
      metadata,
    );
  }

  @Post('logout')
  async logout(@Body() logoutRequestDto: AuthRequestDto): Promise<void> {
    await this.authenticationService.logout(logoutRequestDto.didToken);
  }
}
