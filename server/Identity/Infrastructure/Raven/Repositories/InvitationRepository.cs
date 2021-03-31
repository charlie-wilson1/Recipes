using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Domain;
using Recipes.Identity.Infrastructure.Raven.Indexes.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Identity.Infrastructure.Raven.Repositories
{
    public class InvitationRepository : RepositoryBase<string, ApplicationInvitation>, IInvitationRepository
    {
        public InvitationRepository(IAsyncDocumentSession session) : base(session) { }

        public async Task<ApplicationInvitation> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _session.Query<ApplicationInvitation, Invitations_ByEmail>()
                .SingleOrDefaultAsync(invitation => invitation.Email == email, cancellationToken);
        }

        public async Task DeleteInvitationAsync(string documentId, CancellationToken cancellationToken)
        {
            _session.Delete(documentId);
            await _session.SaveChangesAsync(cancellationToken);
        }
    }
}
