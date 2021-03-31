using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Domain;

namespace Recipes.Identity.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IUserRepository _userRepository;

        public PasswordService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateHash(string password, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public void ValidatePasswordHash(string password, byte[] hash, byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hash[i])
                    {
                        throw new ValidationException();
                    }
                }
            }
        }

        public async Task UpdateAsync(ApplicationUser user, string newPassword, CancellationToken cancellationToken)
        {
            CreateHash(newPassword, out byte[] hash, out byte[] salt);
            user.UpdatePassword(hash, salt);
            await _userRepository.UpdateAsync(user, cancellationToken);
        }
    }
}
