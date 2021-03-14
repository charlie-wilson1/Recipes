using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Recipes.Application.Dtos.Identity.Responses;

namespace Recipes.Application.Contracts.Identity
{
    public interface IIdentityService
    {
        Task<bool> AssertThereWillStillBeAdminsAsync(IList<string> currentRoles);
        Task<bool> IsUserLockedOut(IdentityUser user);
        Task<IdentityUser> FindUserByEmailAsync(string email);
        Task<IdentityUser> FindUserByUsernameAsync(string username);
        Task IncrementFailedLoginAttempts(IdentityUser user);
        Task InsertPasswordlessUserAsync(string email);
        Task ResetFailedLoginAttempts(IdentityUser user);
        Task UpdateEmailAsync(IdentityUser user, string email);
        Task UpdateUsernameAsync(IdentityUser user, string newUsername);
        IdentityUser ValidateUserLogin(IdentityUser user, string password);
        Task<bool> CanUserBeLockedOut(IdentityUser user);
        Task<DateTimeOffset> GetTimeTillUnlocked(IdentityUser user);
        Task<string> FindUsernameByUserIdAsync(string userId);
        Task<List<AdminGetUsersResponseItem>> GetUsersAsync();
        List<AdminGetUsersResponseItem> MapUserRoleDictionaryToUserResponseItem(Dictionary<string, IList<IdentityUser>> usersDict);
        Task<Dictionary<string, IList<IdentityUser>>> CreateUserDictionary(List<string> roles);
    }
}
