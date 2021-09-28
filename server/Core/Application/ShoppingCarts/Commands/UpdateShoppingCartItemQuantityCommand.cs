using MediatR;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Application.ShoppingCarts.Dtos;
using Recipes.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.ShoppingCarts.Commands
{
    public class UpdateShoppingCartItemQuantityCommand : IRequest<ShoppingCart>
    {
        public UpdateShoppingCartItemRequest Item { get; set; }

        public class Handler : IRequestHandler<UpdateShoppingCartItemQuantityCommand, ShoppingCart>
        {
            private readonly IShoppingCartService _shoppingCartService;
            private readonly IShoppingCartRepository _shoppingCartRepository;

            public Handler(IShoppingCartService shoppingCartService, IShoppingCartRepository shoppingCartRepository)
            {
                _shoppingCartService = shoppingCartService;
                _shoppingCartRepository = shoppingCartRepository;
            }

            public async Task<ShoppingCart> Handle(UpdateShoppingCartItemQuantityCommand request, CancellationToken cancellationToken)
            {
                var shoppingCart = await _shoppingCartService.GetShoppingCartByOwnerAsync(cancellationToken);
                _shoppingCartService.UpdateItem(request.Item, shoppingCart.GetItemByName(request.Item.Name));
                await _shoppingCartRepository.UpdateAsync(shoppingCart, cancellationToken);
                return shoppingCart;
            }
        }
    }
}
