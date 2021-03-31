using System.Linq;
using Raven.Client.Documents.Indexes;
using Recipes.Identity.Domain;

namespace Recipes.Identity.Infrastructure.Raven.Indexes.Users
{
    public class Users_ByEmailAndUsername : AbstractIndexCreationTask<ApplicationUser>
    {
        public Users_ByEmailAndUsername()
        {
            Map = users => users
                .Select(user => new { user.Email, user.Username, user.IsActive });
        }
    }
}
