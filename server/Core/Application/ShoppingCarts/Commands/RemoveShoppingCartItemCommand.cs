using MediatR;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.ShoppingCarts.Commands
{
    public class RemoveShoppingCartItemCommand : IRequest<ShoppingCart>
    {
        public string ItemName { get; set; }

        public class Handler : IRequestHandler<RemoveShoppingCartItemCommand, ShoppingCart>
        {
            private readonly IShoppingCartService _shoppingCartService;
            private readonly IShoppingCartRepository _shoppingCartRepository;

            public Handler(IShoppingCartService shoppingCartService, IShoppingCartRepository shoppingCartRepository)
            {
                _shoppingCartService = shoppingCartService;
                _shoppingCartRepository = shoppingCartRepository;
            }

            public async Task<ShoppingCart> Handle(RemoveShoppingCartItemCommand request, CancellationToken cancellationToken)
            {
                var shoppingCart = await _shoppingCartService.GetShoppingCartByOwnerAsync(cancellationToken);
                var item = shoppingCart.GetItemByName(request.ItemName);
                shoppingCart.RemoveItem(item);
                await _shoppingCartRepository.UpdateAsync(shoppingCart, cancellationToken);
                return shoppingCart;
            }
        }
    }
}
