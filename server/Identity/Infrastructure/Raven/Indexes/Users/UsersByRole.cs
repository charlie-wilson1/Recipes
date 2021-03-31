using System.Linq;
using Raven.Client.Documents.Indexes;
using Recipes.Identity.Domain;

namespace Recipes.Identity.Infrastructure.Raven.Indexes.Users
{
    public class Users_ByRole : AbstractIndexCreationTask<ApplicationUser>
    {
        public Users_ByRole()
        {
            Map = users => users
                .Select(user => new { user.Roles, user.IsActive });
        }
    }
}
