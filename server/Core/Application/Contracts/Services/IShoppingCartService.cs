using Recipes.Core.Application.ShoppingCarts.Dtos;
using Recipes.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.Contracts.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetShoppingCartByOwnerAsync(CancellationToken cancellationToken);
        void UpdateItem(UpdateShoppingCartItemRequest request, ShoppingCartItem item);
    }
}
