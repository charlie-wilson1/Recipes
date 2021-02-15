using Microsoft.AspNetCore.Identity;
using Recipes.Application.Common.Exceptions;
using Recipes.Application.Contracts.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.Identity.Services
{
    public class IdentityRoleService : IIdentityRoleService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityRoleService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task EnsureNewRolesExist(List<string> roles)
        {
            var missingRoles = new List<string>();
            foreach (var role in roles)
            {
                var rolesExists = await _roleManager.RoleExistsAsync(role);

                if (!rolesExists)
                {
                    missingRoles.Add(role);
                }
            }

            if (missingRoles.Count > 0)
            {
                throw new NotFoundException($"The following roles do not exist: {string.Join(", ", missingRoles)}");
            }
        }

        public async Task<bool> DoesRoleExistAsync(string role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            return roleExists;
        }

        public async Task<bool> IsUserInRole(IdentityUser user, string role)
        {
            var isInRole = await _userManager.IsInRoleAsync(user, role);
            return isInRole;
        }

        public async Task<IList<string>> UpdateUserRolesAsync(IdentityUser user, List<string> roles)
        {
            var userCurrentRoles = await _userManager.GetRolesAsync(user);

            if (userCurrentRoles is null)
            {
                throw new NullReferenceException("Roles cannot be null");
            }

            var rolesToRemove = userCurrentRoles.Where(x => !roles.Contains(x));
            var rolesToAdd = roles.Where(x => !userCurrentRoles.Contains(x));

            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            await _userManager.AddToRolesAsync(user, rolesToAdd);

            var updatedRoles = await _userManager.GetRolesAsync(user);
            return updatedRoles;
        }

        public async Task<IList<string>> GetUserRolesAsync(IdentityUser user)
        {
            var userCurrentRoles = await _userManager.GetRolesAsync(user);
            return userCurrentRoles;
        }

        public async Task<int> CountUsersInRole(string roleName)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            var count = usersInRole.Count;
            return count;
        }

        public async Task AddRoleAsync(string roleName)
        {
            var role = new IdentityRole(roleName);
            var response = await _roleManager.CreateAsync(role);

            if (!response.Succeeded)
            {
                throw new Exception("Role not added");
            }
        }
    }
}
