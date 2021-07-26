import { ConfigModule } from '@nestjs/config';
import { JwtModule } from '@nestjs/jwt';
import {
  getConnectionToken,
  getModelToken,
  MongooseModule,
} from '@nestjs/mongoose';
import { PassportModule } from '@nestjs/passport';
import { Test, TestingModule } from '@nestjs/testing';
import * as Joi from 'joi';
import { Connection } from 'mongoose';
import { AuthenticationController } from '../../authentication/authentication.controller';
import { AuthenticationService } from '../../authentication/authentication.service';
import { JwtStrategy } from '../../authentication/strategies/jwt.strategy';
import testDbConfig, { closeMongoConnection } from '../../../test/testDbConfig';
import {
  Profile,
  ProfileDocument,
  ProfileSchema,
} from '../models/profile.schema';
import { ProfileModule } from '../profile.module';
import { ProfileService } from '../profile.service';
import { ProfileModelMock } from './support/profileMock.model';
import { getProfileStub } from '../../../test/support/stubs/profile.stub';
import { ProfileRepository } from '../profile.repository';
import { ConflictException } from '@nestjs/common';

// const userServiceMock = new ProfileServiceMock();
const profileMock = getProfileStub();

describe('ProfileService', () => {
  let service: ProfileService;
  let repository: ProfileRepository;
  let connection: Connection;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      controllers: [AuthenticationController],
      imports: [
        ConfigModule.forRoot({
          validationSchema: Joi.object({
            JWT_SECRET: Joi.string().default('secret'),
            JWT_ISSUER: Joi.string().default('issuer'),
          }),
        }),
        JwtModule.registerAsync({
          useFactory: async () => ({
            secretOrPrivateKey: 'test',
            signOptions: {
              expiresIn: '60s',
              issuer: 'issuer',
            },
          }),
        }),
        testDbConfig({
          connectionName: (new Date().getTime() * Math.random()).toString(16),
        }),
        MongooseModule.forFeature([
          {
            name: Profile.name,
            schema: ProfileSchema,
          },
        ]),
        PassportModule,
        ProfileModule,
      ],
      providers: [
        ProfileService,
        ProfileRepository,
        AuthenticationService,
        JwtStrategy,
        {
          provide: getModelToken(Profile.name),
          useClass: ProfileModelMock,
        },
      ],
    }).compile();

    service = module.get<ProfileService>(ProfileService);
    repository = module.get<ProfileRepository>(ProfileRepository);
    connection = await module.get(getConnectionToken());
    jest.clearAllMocks();
  });

  afterAll(async (done) => {
    await connection.close();
    await closeMongoConnection();
    done();
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('create', () => {
    let findByEmailSpy: jest.SpyInstance<Promise<ProfileDocument>>;
    let createSpy: jest.SpyInstance<Promise<ProfileDocument>>;

    beforeEach((done) => {
      findByEmailSpy = jest.spyOn(repository, 'findByEmail');
      createSpy = jest.spyOn(repository, 'create');
      done();
    });

    it('should throw conflict exception when existing user not found', async () => {
      findByEmailSpy.mockResolvedValue(null);
      const error = () => service.create({ email: profileMock.email });
      expect(error).toThrow(ConflictException);
      expect(repository.findByEmail).toHaveBeenCalledWith(profileMock.email);
    });
  });
});
