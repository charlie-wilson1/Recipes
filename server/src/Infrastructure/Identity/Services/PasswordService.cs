using Microsoft.AspNetCore.Identity;
using Recipes.Application.Common.Exceptions;
using Recipes.Application.Contracts.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.Identity.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public PasswordService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidatePasswordAsync(IdentityUser user, string password)
        {
            var passwordValidator = new PasswordValidator<IdentityUser>();
            var result = await passwordValidator.ValidateAsync(_userManager, user, password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                throw new ValidationException(errors);
            }
        }

        public async Task<bool> CheckIfPasswordValid(IdentityUser user, string password)
        {
            var passwordValidator = new PasswordValidator<IdentityUser>();
            var result = await passwordValidator.ValidateAsync(_userManager, user, password);
            return result.Succeeded;
        }

        public async Task<bool> CheckIfUserHasPassword(IdentityUser user)
        {
            var result = await _userManager.HasPasswordAsync(user);
            return result;
        }

        public async Task AddPasswordAsync(IdentityUser user, string password)
        {
            var updateResult = await _userManager.AddPasswordAsync(user, password);

            if (!updateResult.Succeeded)
            {
                throw new ArgumentException("updating password failed", string.Join(", ", updateResult.Errors));
            }
        }

        public async Task UpdatePasswordAsync(IdentityUser user, string currentPassword, string newPassword)
        {
            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, currentPassword);

            if (!isCorrectPassword)
            {
                throw new UnauthorizedAccessException("incorrect password");
            }

            var updateResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!updateResult.Succeeded)
            {
                throw new ArgumentException("updating password failed", string.Join(", ", updateResult.Errors));
            }
        }

        public async Task ResetPasswordAsync(IdentityUser user, string resetToken, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (!result.Succeeded)
            {
                throw new Exception("Resetting password failed.");
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(IdentityUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }
    }
}
