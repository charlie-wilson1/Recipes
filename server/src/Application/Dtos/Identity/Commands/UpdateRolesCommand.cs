using MediatR;
using Recipes.Application.Contracts.Identity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Commands
{
    public class UpdateRolesCommand : IRequest<IList<string>>
    {
        public string Username { get; set; }
        public List<string> Roles { get; set; }

        public class Handler : IRequestHandler<UpdateRolesCommand, IList<string>>
        {
            private readonly IIdentityService _identityService;
            private readonly IIdentityRoleService _roleService;

            public Handler(IIdentityService identityService, IIdentityRoleService roleService)
            {
                _identityService = identityService;
                _roleService = roleService;
            }

            public async Task<IList<string>> Handle(UpdateRolesCommand request, CancellationToken cancellationToken)
            {
                await _roleService.EnsureNewRolesExist(request.Roles);
                var user = await _identityService.FindUserByUsernameAsync(request.Username);
                var currentRoles = await _roleService.GetUserRolesAsync(user);
                var thereWillBeAdmins = await _identityService.AssertThereWillStillBeAdminsAsync(currentRoles);

                if (!thereWillBeAdmins)
                {
                    throw new ArgumentException("There must be admins for the application to function.");
                }

                var roles = await _roleService.UpdateUserRolesAsync(user, request.Roles);

                return roles;
            }
        }
    }
}
