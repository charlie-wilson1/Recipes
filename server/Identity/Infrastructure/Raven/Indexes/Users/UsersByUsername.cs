using Raven.Client.Documents.Indexes;
using Recipes.Identity.Domain;
using System.Linq;

namespace Recipes.Identity.Infrastructure.Raven.Indexes.Users
{
    public class Users_ByUsername : AbstractIndexCreationTask<ApplicationUser>
    {
        public Users_ByUsername()
        {
            Map = users => users
                .Select(user => new { user.Username, user.IsActive });
        }
    }
}
