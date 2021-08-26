import {
  Body,
  Controller,
  Inject,
  LoggerService,
  Post,
  UnauthorizedException,
} from '@nestjs/common';
import { WINSTON_MODULE_NEST_PROVIDER } from 'nest-winston';
import { AuthenticationService } from './authentication.service';
import { AuthRequestDto } from './models/loginRequestDto';

@Controller('authorization')
export class AuthenticationController {
  constructor(
    private readonly authenticationService: AuthenticationService,

    @Inject(WINSTON_MODULE_NEST_PROVIDER)
    private readonly logger: LoggerService,
  ) {}

  @Post('login')
  async authenticate(@Body() loginRequestDto: AuthRequestDto): Promise<any> {
    const metadata = await this.authenticationService.authenticateDidToken(
      loginRequestDto.didToken,
    );

    if (!metadata) {
      this.logger.log('didToken invalid.', AuthenticationController.name);
      await this.authenticationService.logout(loginRequestDto.didToken);
      throw new UnauthorizedException();
    }

    const user = await this.authenticationService.getUser(metadata.email);

    if (!user?.isActive) {
      this.logger.log(
        `User with email ${metadata.email} is inactive.`,
        AuthenticationController.name,
      );
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
