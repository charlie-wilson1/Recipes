import { Logger, Module } from '@nestjs/common';
import { ConfigModule, ConfigService } from '@nestjs/config';
import { JwtModule } from '@nestjs/jwt';
import { PassportModule } from '@nestjs/passport';
import { UserModule } from '../user/user.module';
import { JwtStrategy } from '../authentication/strategies/jwt.strategy';
import { AuthenticationController } from './authentication.controller';
import { AuthenticationService } from './authentication.service';
import { TypeOrmModule } from '@nestjs/typeorm';
import { UserRepository } from 'src/user/user.repository';

@Module({
  controllers: [AuthenticationController],
  imports: [
    JwtModule.registerAsync({
      imports: [ConfigModule],
      useFactory: async (configService: ConfigService) => ({
        secret: configService.get<string>('JWT_SECRET'),
        signOptions: {
          expiresIn: '1h',
          issuer: configService.get<string>('JWT_ISSUER'),
          audience: configService.get<string>('JWT_AUDIENCE'),
        },
      }),
      inject: [ConfigService],
    }),
    TypeOrmModule.forFeature([UserRepository]),
    PassportModule.register({ defaultStrategy: 'jwt' }),
    ConfigModule,
    UserModule,
  ],
  providers: [AuthenticationService, JwtStrategy, Logger],
  exports: [JwtStrategy, PassportModule],
})
export class AuthenticationModule {}
