using MediatR;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.ShoppingCarts.Commands
{
    public class ClearShoppingCartCommand : IRequest
    {
        public class Handler : IRequestHandler<ClearShoppingCartCommand>
        {
            private readonly IShoppingCartService _shoppingCartService;
            private readonly IShoppingCartRepository _shoppingCartRepository;

            public Handler(IShoppingCartService shoppingCartService, IShoppingCartRepository shoppingCartRepository)
            {
                _shoppingCartService = shoppingCartService;
                _shoppingCartRepository = shoppingCartRepository;
            }

            public async Task<Unit> Handle(ClearShoppingCartCommand request, CancellationToken cancellationToken)
            {
                var shoppingCart = await _shoppingCartService.GetShoppingCartByOwnerAsync(cancellationToken);
                shoppingCart.EmptyCart();
                await _shoppingCartRepository.UpdateAsync(shoppingCart, cancellationToken);
                return Unit.Value;
            }
        }
    }
}
