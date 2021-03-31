using Recipes.Identity.Application.Identity.Commands;
using Recipes.Identity.Application.Identity.Responses;
using Recipes.Identity.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Identity.Application.Contracts.Services
{
    public interface IUserService
    {
        UserWithTokenResponse CreateUserWithTokenResponse(ApplicationUser user);
        Task<ApplicationUser> GetByEmail(string email, CancellationToken cancellationToken);
        Task<ApplicationUser> GetByUsername(string username, CancellationToken cancellationToken);
        Task<UserWithTokenResponse> Insert(RegisterCommand request, CancellationToken cancellationToken);
        ApplicationUser ManageCreation(string username, string email, string password);
        Task ValidateNewEmail(string email, CancellationToken cancellationToken);
        Task ValidateNewUsername(string username, CancellationToken cancellationToken);
        void ValidateRefreshToken(ApplicationUser user, string refreshToken);
    }
}
