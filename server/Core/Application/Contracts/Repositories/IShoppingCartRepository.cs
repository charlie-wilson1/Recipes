using Recipes.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.Contracts.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetByOwnerAsync(string owner, CancellationToken cancellationToken);
        Task InsertAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken);
        Task UpdateAsync(ShoppingCart shoppingCart, CancellationToken cancellation);
    }
}
