using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Domain;

namespace Recipes.Identity.Application.Identity.Commands
{
    public class AdminUpdateUserRolesCommand : IRequest
    {
        public string Username { get; set; }
        public List<string> Roles { get; set; }

        public class Handler : IRequestHandler<AdminUpdateUserRolesCommand>
        {
            private readonly IUserRepository _userRepository;
            private readonly string ADMIN_ROLE = "Admin";

            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<Unit> Handle(AdminUpdateUserRolesCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);

                if (!IsSafeToContinue(user.Roles, request.Roles))
                {
                    await ValidateRoleChange(user, request.Roles, cancellationToken);
                }

                await UpdateRoles(user, request.Roles, cancellationToken);
                return Unit.Value;
            }

            public async Task ValidateRoleChange(ApplicationUser user, List<string> roles, CancellationToken cancellationToken)
            {
                var admins = await _userRepository.GetByRoleAsync(ADMIN_ROLE, cancellationToken);

                if (admins.Count() <= 1)
                {
                    throw new ArgumentException("There must be admins to continue this application.");
                }
            }

            public async Task UpdateRoles(ApplicationUser user, List<string> roles, CancellationToken cancellationToken)
            {
                user.UpdateRoles(roles);
                await _userRepository.UpdateAsync(user, cancellationToken);
            }

            private bool IsSafeToContinue(List<string> currentRoles, List<string> newRoles)
            {
                return newRoles.Contains(ADMIN_ROLE) || !currentRoles.Contains(ADMIN_ROLE);
            }
        }
    }
}
