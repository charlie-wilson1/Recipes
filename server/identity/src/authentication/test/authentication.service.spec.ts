import { JwtModule, JwtService } from '@nestjs/jwt';
import { Test, TestingModule } from '@nestjs/testing';
import { AuthenticationService } from '../authentication.service';
import { UserRepository } from '../../user/user.repository';
import { User } from '../../user/entities/user.entity';
import { Role } from '../../models/roles';

const mockUserRepository = () => ({
  findOne: jest.fn(),
});

const mockUser: User = {
  _id: '1',
  id: '1',
  username: 'username',
  roles: [Role.Member],
  email: 'test@test.com',
  isActive: true,
};

describe('AuthenticationService', () => {
  let service: AuthenticationService;
  let userRepository: UserRepository;
  let jwtService: JwtService;

  beforeEach(async (done) => {
    const module: TestingModule = await Test.createTestingModule({
      imports: [
        JwtModule.registerAsync({
          useFactory: async () => ({
            secretOrPrivateKey: 'test',
            signOptions: {
              expiresIn: '60s',
              issuer: 'issuer',
            },
          }),
        }),
      ],
      providers: [
        AuthenticationService,
        { provide: UserRepository, useFactory: mockUserRepository },
      ],
    }).compile();

    service = module.get<AuthenticationService>(AuthenticationService);
    userRepository = module.get<UserRepository>(UserRepository);
    jwtService = module.get<JwtService>(JwtService);
    jest.clearAllMocks();
    done();
  });

  test('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('getUser', () => {
    let findOneSpy: jest.SpyInstance;

    beforeEach((done) => {
      findOneSpy = jest.spyOn(userRepository, 'findOne');
      done();
    });

    test('should call findByEmail', async () => {
      findOneSpy.mockImplementation(() => Promise.resolve(mockUser));
      await service.getUser(mockUser.email);
      expect(findOneSpy).toHaveBeenCalledTimes(1);

      expect(userRepository.findOne).toHaveBeenCalledWith({
        email: mockUser.email,
      });
    });
  });

  describe('createJwtFromUser', () => {
    let signSpy: jest.SpyInstance;

    beforeEach((done) => {
      signSpy = jest.spyOn(jwtService, 'sign');
      done();
    });

    test('should sign jwt with username and roles', async () => {
      const expectedResult = 'jwtToken';
      signSpy.mockImplementation(() => expectedResult);
      const actual = service.createJwtFromUser(mockUser);
      expect(signSpy).toHaveBeenCalledTimes(1);
      expect(actual).toEqual(expectedResult);

      expect(jwtService.sign).toHaveBeenCalledWith({
        roles: mockUser.roles,
        name: mockUser.username,
        nameid: mockUser.email,
      });
    });
  });
});
