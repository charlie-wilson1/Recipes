using Recipes.Identity.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Identity.Application.Contracts.Services
{
    public interface IPasswordService
    {
        void CreateHash(string password, out byte[] hash, out byte[] salt);
        Task UpdateAsync(ApplicationUser user, string newPassword, CancellationToken cancellationToken);
        void ValidatePasswordHash(string password, byte[] hash, byte[] salt);
    }
}
