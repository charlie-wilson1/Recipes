using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Recipes.Core.Application.Common.Models;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Domain;
using Recipes.Core.Infrastructure.Raven.Indexes;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Recipes.Core.Infrastructure.Raven.Repositories
{
    public class RecipesRepository : IRecipeRepository
    {
        protected readonly IAsyncDocumentSession _session;
        private readonly IDateTime _dateTime;

        public RecipesRepository(IAsyncDocumentSession session, IDateTime dateTime)
        {
            _session = session;
            _dateTime = dateTime;
        }

        public  async Task<Recipe> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _session.LoadAsync<Recipe>(id, cancellationToken);
        }

        public async Task InsertAsync(Recipe entity, CancellationToken cancellationToken)
        {
            await _session.StoreAsync(entity, cancellationToken);
            await _session.SaveChangesAsync(cancellationToken);
        }

        public async Task<PaginatedResponse<Recipe>> GetActiveAsync(string username, PaginatedRequest request, CancellationToken cancellationToken)
        {
            var ravenQuery = $"*{request.SearchQuery}*";
            var query = await _session.Query<Recipe, Recipes_ByUserAndName>()
                .Statistics(out QueryStatistics stats)
                .Where(recipe => recipe.IsDeleted == false)
                .Search(recipe => recipe.Name, ravenQuery, boost: 10)
                .Search(recipe => recipe.Owner, username, boost: 5)
                .Search(recipe => recipe.Owner, ravenQuery, boost: 3)
                .OrderByScore()
                .ThenBy(x => x.Name)
                .Skip(((request.PageNumber - 1) * request.PageSize) + request.SkippedResults)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResponse<Recipe>
            {
                Total = stats.TotalResults,
                Data = query
            };
        }

        public async Task UpdateAsync(Recipe recipe, CancellationToken cancellation)
        {
            var dbRecipe = await _session.LoadAsync<Recipe>(recipe.Id, cancellation);
            dbRecipe.Update(recipe, _dateTime.Now);
            await _session.SaveChangesAsync(cancellation);
        }
    }
}
