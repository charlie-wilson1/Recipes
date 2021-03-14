using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Recipes.Application.Contracts.Identity
{
    public interface IIdentityRoleService
    {
        Task AddRoleAsync(string roleName);
        Task<int> CountUsersInRole(string roleName);
        Task<bool> DoesRoleExistAsync(string role);
        Task EnsureNewRolesExist(List<string> roles);
        List<string> GetAllRoles();
        Task<IList<string>> GetUserRolesAsync(IdentityUser user);
        Task<bool> IsUserInRole(IdentityUser user, string role);
        Task<IList<string>> UpdateUserRolesAsync(IdentityUser user, List<string> roles);
    }
}
