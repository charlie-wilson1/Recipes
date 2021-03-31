using Raven.Client.Documents.Indexes;
using Recipes.Identity.Domain;
using System.Linq;

namespace Recipes.Identity.Infrastructure.Raven.Indexes.Users
{
    public class Users_ByEmail : AbstractIndexCreationTask<ApplicationUser>
    {
        public Users_ByEmail()
        {
            Map = users => users
                .Select(user => new { user.Email, user.IsActive });
        }
    }
}
