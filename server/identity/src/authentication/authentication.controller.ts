import { Body, Controller, Post, UnauthorizedException } from '@nestjs/common';
import { AuthenticationService } from './authentication.service';
import { AuthRequestDto } from './models/loginRequestDto';

@Controller('authorization')
export class AuthenticationController {
  constructor(private readonly authenticationService: AuthenticationService) {}

  @Post('login')
  async authenticate(@Body() loginRequestDto: AuthRequestDto): Promise<any> {
    const metadata = await this.authenticationService.authenticateDidToken(
      loginRequestDto.didToken,
    );

    if (!metadata) {
      await this.authenticationService.logout(loginRequestDto.didToken);
      throw new UnauthorizedException();
    }

    const user = await this.authenticationService.getUser(metadata.email);

    if (!user?.isActive) {
      await this.authenticationService.logout(loginRequestDto.didToken);
      throw new UnauthorizedException(
        `User with email ${metadata.email} is not authorized to use this application. Please contact an administrator to be invited to use the application.`,
      );
    }

    return await this.authenticationService.createJwtFromUser(user);
  }

  @Post('logout')
  async logout(@Body() logoutRequestDto: AuthRequestDto): Promise<void> {
    await this.authenticationService.logout(logoutRequestDto.didToken);
  }
}
