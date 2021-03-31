using Recipes.Identity.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Identity.Application.Contracts.Repositories
{
    public interface IInvitationRepository : IRepository<string, ApplicationInvitation>
    {
        Task DeleteInvitationAsync(string email, CancellationToken cancellationToken);
        Task<ApplicationInvitation> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
