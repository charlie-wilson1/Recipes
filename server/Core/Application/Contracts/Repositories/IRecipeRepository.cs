using Recipes.Core.Application.Common.Models;
using Recipes.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.Contracts.Repositories
{
    public interface IRecipeRepository
    {
        Task<PaginatedResponse<Recipe>> GetActiveAsync(string username, PaginatedRequest request, CancellationToken cancellationToken);
        Task<Recipe> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task InsertAsync(Recipe entity, CancellationToken cancellationToken);
        Task UpdateAsync(Recipe recipe, CancellationToken cancellation);
    }
}
