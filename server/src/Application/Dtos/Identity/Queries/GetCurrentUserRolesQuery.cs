using MediatR;
using Recipes.Application.Contracts.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Queries
{
    public class GetCurrentUserRolesQuery : IRequest<IList<string>>
    {
        public class Handler : IRequestHandler<GetCurrentUserRolesQuery, IList<string>>
        {
            private readonly IIdentityService _identityService;
            private readonly IIdentityRoleService _roleService;
            private readonly ICurrentUserService _currentUserService;

            public Handler(
                IIdentityService identityService,
                IIdentityRoleService roleService,
                ICurrentUserService currentUserService)
            {
                _identityService = identityService;
                _roleService = roleService;
                _currentUserService = currentUserService;
            }

            public async Task<IList<string>> Handle(GetCurrentUserRolesQuery request, CancellationToken cancellationToken)
            {
                var username = _currentUserService.Username;
                var user = await _identityService.FindUserByUsernameAsync(username);
                var roles = await _roleService.GetUserRolesAsync(user);
                return roles;
            }
        }
    }
}
