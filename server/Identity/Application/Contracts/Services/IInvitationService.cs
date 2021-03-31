using Recipes.Identity.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Identity.Application.Contracts.Services
{
    public interface IInvitationService
    {
        ApplicationInvitation Create(string email, string token);
        Task SendInvitationEmail(string email, string token, CancellationToken cancellationToken);
        Task ValidateNewEmail(string email, CancellationToken cancellationToken);
        Task ValidateToken(string email, string token, CancellationToken cancellationToken);
    }
}
