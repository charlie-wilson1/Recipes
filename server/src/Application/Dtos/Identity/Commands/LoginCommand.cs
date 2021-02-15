using MediatR;
using Microsoft.AspNetCore.Identity;
using Recipes.Application.Contracts;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Dtos.Identity.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Commands
{
    public class LoginCommand : IRequest<JwtAuthResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class Handler : IRequestHandler<LoginCommand, JwtAuthResponse>
        {
            private readonly IIdentityService _identityService;
            private readonly IJwtService _jwtService;
            private readonly IIdentityRoleService _roleService;
            private readonly IDateTime _dateTime;

            public Handler(
                IIdentityService identityService,
                IJwtService jwtService,
                IIdentityRoleService roleService, 
                IDateTime dateTime)
            {
                _identityService = identityService;
                _jwtService = jwtService;
                _roleService = roleService;
                _dateTime = dateTime;
            }

            public async Task<JwtAuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindUserByUsernameAsync(request.Username);
                var isLockedOut = await _identityService.IsUserLockedOut(user);

                if (isLockedOut)
                {
                    var offset = await _identityService.GetTimeTillUnlocked(user);
                    var minutesRemaining = offset.Subtract(_dateTime.Now).Minutes;
                    throw new ArgumentException($"User is locked out for {minutesRemaining} minutes.");
                }

                try
                {
                    _identityService.ValidateUserLogin(user, request.Password);
                }
                catch
                {
                    var canUserBeLockedOut = await _identityService.CanUserBeLockedOut(user);

                    if (canUserBeLockedOut)
                    {
                        await _identityService.IncrementFailedLoginAttempts(user);
                    }

                    throw;
                }

                await _identityService.ResetFailedLoginAttempts(user);
                var roles = await _roleService.GetUserRolesAsync(user);
                var tokenResponse = await _jwtService.GenerateJwtTokens(user, roles);
                return tokenResponse;
            }
        }
    }
}
