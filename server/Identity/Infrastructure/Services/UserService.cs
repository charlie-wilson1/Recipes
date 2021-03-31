using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Recipes.Identity.Application.Common.Exceptions;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Application.Identity.Commands;
using Recipes.Identity.Application.Identity.Responses;
using Recipes.Identity.Domain;

using ValidationException = Recipes.Identity.Application.Common.Exceptions.ValidationException;

namespace Recipes.Identity.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public UserService(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService, IMapper mapper, IDateTime dateTime)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _mapper = mapper;
            _dateTime = dateTime;
        }

        public async Task ValidateNewEmail(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user != null)
            {
                throw new ValidationException("Email already exists.");
            }
        }

        public async Task ValidateNewUsername(string username, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(username, cancellationToken);

            if (user != null)
            {
                throw new ValidationException("Username already exists.");
            }
        }

        public void ValidateRefreshToken(ApplicationUser user, string refreshToken)
        {
            if (!user.IsRefreshTokenValid(refreshToken))
            {
                throw new SecurityTokenValidationException();
            }
        }

        public async Task<ApplicationUser> GetById(string id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("user");
            }

            return user;
        }

        public async Task<ApplicationUser> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("user", email);
            }

            return user;
        }

        public async Task<ApplicationUser> GetByUsername(string username, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(username, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("user", username);
            }

            return user;
        }

        public async Task<UserWithTokenResponse> Insert(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = ManageCreation(request.Username, request.Email, request.Password);
            await _userRepository.InsertAsync(user, cancellationToken);
            return CreateUserWithTokenResponse(user);
        }

        public ApplicationUser ManageCreation(string username, string email, string password)
        {
            var user = new ApplicationUser();
            _passwordService.CreateHash(password, out byte[] hash, out byte[] salt);
            var token = _jwtService.GenerateRandomToken();
            user.CreateUser(username, email, token, hash, salt, _dateTime.Now);
            return user;
        }

        public UserWithTokenResponse CreateUserWithTokenResponse(ApplicationUser user)
        {
            var response = _mapper.Map<UserWithTokenResponse>(user);
            response.Jwt = _jwtService.WriteJwtToken(user);
            return response;
        }
    }
}
