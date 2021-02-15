using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Recipes.Application.Common.Exceptions;
using Recipes.Application.Contracts.Identity;
using Recipes.Infrastructure.Identity.Models;

namespace Recipes.Infrastructure.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityRoleService _roleService;

        public IdentityService(
            UserManager<IdentityUser> userManager,
            IIdentityRoleService roleService)
        {
            _userManager = userManager;
            _roleService = roleService;
        }

        public async Task<IdentityUser> FindUserByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
            {
                throw new NotFoundException("user", username);
            }

            return user;
        }

        public async Task<IdentityUser> FindUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                throw new NotFoundException("user");
            }

            return user;
        }

        public async Task<string> FindUsernameByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user is null)
            {
                throw new NotFoundException("user");
            }

            return user.UserName;
        }

        public async Task InsertPasswordlessUserAsync(string email)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                NormalizedEmail = _userManager.NormalizeEmail(email),
                LockoutEnabled = true
            };

            var createResult = await _userManager.CreateAsync(user);

            if (!createResult.Succeeded)
            {
                throw new Exception("Creating user failed.");
            }
            
            var rolesResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!rolesResult.Succeeded)
            {
                throw new Exception("Adding roles failed.");
            }
        }

        public async Task UpdateUsernameAsync(IdentityUser user, string newUsername)
        {
            var updateUsernameResult = await _userManager.SetUserNameAsync(user, newUsername);

            if (!updateUsernameResult.Succeeded)
            {
                throw new Exception("updating username failed");
            }

            await _userManager.UpdateNormalizedUserNameAsync(user);
        }

        public async Task UpdateEmailAsync(IdentityUser user, string email)
        {
            var updateResult = await _userManager.SetEmailAsync(user, email);

            if (!updateResult.Succeeded)
            {
                throw new Exception("updating email failed");
            }

            await _userManager.UpdateNormalizedEmailAsync(user);
        }

        public IdentityUser ValidateUserLogin(IdentityUser user, string password)
        {
            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new ArgumentException("Incorrect password.");
            }

            return user;
        }

        public async Task<bool> AssertThereWillStillBeAdminsAsync(IList<string> currentRoles)
        {
            if (!currentRoles.Contains("Admin"))
            {
                return true;
            }

            var result = await _roleService.CountUsersInRole("Admin");
            return result >= 1;
        }

        public async Task IncrementFailedLoginAttempts(IdentityUser user)
        {
            var result = await _userManager.AccessFailedAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception();
            }
        }

        public async Task ResetFailedLoginAttempts(IdentityUser user)
        {
            var result = await _userManager.ResetAccessFailedCountAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception();
            }
        }

        public async Task<bool> CanUserBeLockedOut(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsUserLockedOut(IdentityUser user)
        {
            var isLockedOut = await _userManager.IsLockedOutAsync(user);
            return isLockedOut;
        }

        public async Task SetUserLockedOutStatus(IdentityUser user, bool lockoutStatus)
        {
            var result = await _userManager.SetLockoutEnabledAsync(user, lockoutStatus);

            if (!result.Succeeded)
            {
                throw new Exception();
            }
        }

        public async Task<DateTimeOffset> GetTimeTillUnlocked(IdentityUser user)
        {
            var timespan = await _userManager.GetLockoutEndDateAsync(user);

            if (!timespan.HasValue)
            {
                throw new Exception();
            }

            return timespan.Value;
        }
    }
}
