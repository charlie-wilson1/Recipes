using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Recipes.Identity.Domain;

namespace Recipes.Identity.Application.Contracts.Repositories
{
    public interface IUserRepository : IRepository<string, ApplicationUser>
    {
        Task<IEnumerable<ApplicationUser>> GetActiveAsync(string searchQuery, CancellationToken cancellationToken);
        Task<ApplicationUser> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<IEnumerable<ApplicationUser>> GetByRoleAsync(string role, CancellationToken cancellationToken);
        Task<ApplicationUser> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        Task UpdateAsync(ApplicationUser user, CancellationToken cancellationToken);
    }
}
