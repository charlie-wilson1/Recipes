using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Infrastructure.Raven.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        protected readonly IAsyncDocumentSession _session;

        public ShoppingCartRepository(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public async Task<ShoppingCart> GetByOwnerAsync(string owner, CancellationToken cancellationToken)
        {
            return await _session.Query<ShoppingCart>()
                .Where(x => x.Owner == owner)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task InsertAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken)
        {
            await _session.StoreAsync(shoppingCart, cancellationToken);
            await _session.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(ShoppingCart shoppingCart, CancellationToken cancellation)
        {
            var dbRecipe = await _session.LoadAsync<ShoppingCart>(shoppingCart.Id, cancellation);
            dbRecipe.UpsertItems(shoppingCart.Items);
            await _session.SaveChangesAsync(cancellation);
        }
    }
}
