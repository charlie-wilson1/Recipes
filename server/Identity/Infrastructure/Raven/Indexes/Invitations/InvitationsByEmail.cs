using Raven.Client.Documents.Indexes;
using Recipes.Identity.Domain;
using System.Linq;

namespace Recipes.Identity.Infrastructure.Raven.Indexes.Users
{
    public class Invitations_ByEmail : AbstractIndexCreationTask<ApplicationInvitation>
    {
        public Invitations_ByEmail()
        {
            Map = invitations => invitations
                .Select(invitation => new { invitation.Email });
        }
    }
}
