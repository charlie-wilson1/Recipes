using Recipes.Core.Application.Common.Exceptions;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Application.ShoppingCarts.Dtos;
using Recipes.Core.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartService(ICurrentUserService currentUserService, IShoppingCartRepository shoppingCartRepository)
        {
            _currentUserService = currentUserService;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<ShoppingCart> GetShoppingCartByOwnerAsync(CancellationToken cancellationToken)
        {
            var shoppingCart = await _shoppingCartRepository.GetByOwnerAsync(_currentUserService.Username, cancellationToken);

            if (shoppingCart is null)
            {
                throw new NotFoundException(nameof(ShoppingCart), _currentUserService.Username);
            }

            shoppingCart.Items = shoppingCart.Items
                .OrderBy(x => x.Name)
                .ToList();

            return shoppingCart;
        }

        public void UpdateItem(UpdateShoppingCartItemRequest request, ShoppingCartItem item)
        {
            if (item is null)
            {
                throw new NotFoundException(nameof(ShoppingCartItem), request.Name);
            }

            item.UpdateQuantity(request.Quantity);
        }
    }
}
