using MediatR;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Dtos.Identity.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Commands
{
    public class RefreshJwtTokenCommand : IRequest<JwtAuthResponse>
    {
        public string Username { get; set; }
        public string RefreshToken { get; set; }

        public class Handler : IRequestHandler<RefreshJwtTokenCommand, JwtAuthResponse>
        {
            private readonly IIdentityService _identityService;
            private readonly IIdentityRoleService _roleService;
            private readonly IJwtService _jwtService;

            public Handler(
                IIdentityService identityService,
                IIdentityRoleService roleService,
                IJwtService jwtService)
            {
                _identityService = identityService;
                _roleService = roleService;
                _jwtService = jwtService;
            }

            public async Task<JwtAuthResponse> Handle(RefreshJwtTokenCommand request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindUserByUsernameAsync(request.Username);
                await _jwtService.ValidateRefreshToken(user, request.RefreshToken);
                var roles = await _roleService.GetUserRolesAsync(user);
                var tokenResponse = await _jwtService.GenerateJwtTokens(user, roles);

                return tokenResponse;
            }
        }
    }
}
