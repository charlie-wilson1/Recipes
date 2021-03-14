using MediatR;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Dtos.Identity.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Queries
{
    public class AdminGetUsersQuery : IRequest<List<AdminGetUsersResponseItem>>
    {
        public class Handler : IRequestHandler<AdminGetUsersQuery, List<AdminGetUsersResponseItem>>
        {
            private readonly IIdentityService _identityService;
            private readonly IIdentityRoleService _roleService;

            public Handler(IIdentityService identityService, IIdentityRoleService roleService)
            {
                _identityService = identityService;
                _roleService = roleService;
            }

            public async Task<List<AdminGetUsersResponseItem>> Handle(AdminGetUsersQuery request, CancellationToken cancellationToken)
            {
                var response = await _identityService.GetUsersAsync();
                return response;
            }
        }
    }
}
