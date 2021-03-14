using MediatR;
using Recipes.Application.Contracts.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Queries
{
    public class AdminGetRolesQuery : IRequest<List<string>>
    {
        public class Handler : IRequestHandler<AdminGetRolesQuery, List<string>>
        {
            private readonly IIdentityRoleService _roleService;

            public Handler(IIdentityRoleService roleService)
            {
                _roleService = roleService;
            }

            public async Task<List<string>> Handle(AdminGetRolesQuery request, CancellationToken cancellationToken)
            {
                var response = _roleService.GetAllRoles();
                return response;
            }
        }
    }
}
