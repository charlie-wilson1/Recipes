using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Domain.Generic;

namespace Recipes.Identity.Infrastructure.Raven.Repositories
{
    public abstract class RepositoryBase<TKey, TEntity> : IRepository<TKey, TEntity> 
        where TKey : class 
        where TEntity : EntityWithStringId
    {
        protected readonly IAsyncDocumentSession _session;

        public RepositoryBase(IAsyncDocumentSession session) => _session = session;

        public virtual async Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _session.LoadAsync<TEntity>(id, cancellationToken);
        }

        public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _session.StoreAsync(entity, cancellationToken);
            await _session.SaveChangesAsync(cancellationToken);
        }
    }
}
