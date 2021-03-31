using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Domain;
using Recipes.Identity.Infrastructure.Raven.Indexes.Users;

namespace Recipes.Identity.Infrastructure.Raven.Repositories
{
    public class UserRepository : RepositoryBase<string, ApplicationUser>, IUserRepository
    {
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;

        public UserRepository(
            IAsyncDocumentSession session,
            IDateTime dateTime,
            ICurrentUserService currentUserService) : base(session)
        {
            _dateTime = dateTime;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<ApplicationUser>> GetActiveAsync(string searchQuery, CancellationToken cancellationToken)
        {
            var ravenQuery = $"*{searchQuery}*";
            return await _session.Query<ApplicationUser, Users_ByEmailAndUsername>()
                .Where(user => user.IsActive)
                .Search(user => user.Username, ravenQuery)
                .Search(user => user.Email, ravenQuery, options: SearchOptions.Or)
                .OrderByDescending(user => user.Username)
                .ToListAsync(cancellationToken);
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _session.Query<ApplicationUser, Users_ByEmail>()
                .SingleOrDefaultAsync(user => user.Email == email && user.IsActive, cancellationToken);
        }

        public async Task<ApplicationUser> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await _session.Query<ApplicationUser, Users_ByUsername>()
                .SingleOrDefaultAsync(user => user.Username == username && user.IsActive, cancellationToken);
        }

        public async Task UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var userToUpdate = await _session.LoadAsync<ApplicationUser>(user.Id, cancellationToken);
            userToUpdate.UpdateUser(user, _currentUserService.UserId, _dateTime.UtcNow);
            await _session.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<ApplicationUser>> GetByRoleAsync(string role, CancellationToken cancellationToken)
        {
            return await _session.Query<ApplicationUser, Users_ByRole>()
                .Where(user => user.IsActive)
                .Search(user => user.Roles, role)
                .OrderByDescending(user => user.Username)
                .ToListAsync(cancellationToken);
        }
    }
}
