using MediatR;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Dtos.Identity.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Commands
{
    public class RegisterUserCommand : IRequest<JwtAuthResponse>
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public class Handler : IRequestHandler<RegisterUserCommand, JwtAuthResponse>
        {
            private readonly IIdentityService _identityService;
            private readonly IJwtService _jwtService;
            private readonly IPasswordService _passwordService;
            private readonly IIdentityRoleService _roleService;

            public Handler(
                IIdentityService identityService,
                IJwtService jwtService,
                IPasswordService passwordService, 
                IIdentityRoleService roleService)
            {
                _identityService = identityService;
                _jwtService = jwtService;
                _passwordService = passwordService;
                _roleService = roleService;
            }

            public async Task<JwtAuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                // Verify user can be updated safely
                var user = await _identityService.FindUserByEmailAsync(request.Email);
                var userHasPassword = await _passwordService.CheckIfUserHasPassword(user);

                if (userHasPassword)
                {
                    throw new ArgumentException("User is already registered.");
                }

                await _passwordService.ValidatePasswordAsync(user, request.Password);

                // updates
                await _identityService.UpdateUsernameAsync(user, request.Username);
                await _passwordService.AddPasswordAsync(user, request.Password);

                // login
                var roles = await _roleService.GetUserRolesAsync(user);
                var tokenResponse = await _jwtService.GenerateJwtTokens(user, roles);

                return tokenResponse;
            }
        }
    }
}
