using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Recipes.Application.Contracts.Identity
{
    public interface IPasswordService
    {
        Task AddPasswordAsync(IdentityUser user, string password);
        Task<bool> CheckIfPasswordValid(IdentityUser user, string password);
        Task<bool> CheckIfUserHasPassword(IdentityUser user);
        Task<string> GeneratePasswordResetTokenAsync(IdentityUser user);
        Task ResetPasswordAsync(IdentityUser user, string resetToken, string newPassword);
        Task UpdatePasswordAsync(IdentityUser user, string currentPassword, string newPassword);
        Task ValidatePasswordAsync(IdentityUser user, string password);
    }
}
