using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Identity.Application.Contracts.Repositories
{
    public interface IRepository<TKey, TEntity>
    {
        Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task InsertAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
