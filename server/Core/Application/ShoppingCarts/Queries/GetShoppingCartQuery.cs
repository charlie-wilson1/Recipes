using MediatR;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.ShoppingCarts.Queries
{
    public class GetShoppingCartQuery : IRequest<ShoppingCart>
    {
        public class Handler : IRequestHandler<GetShoppingCartQuery, ShoppingCart>
        {
            private readonly IShoppingCartService _shoppingCartService;

            public Handler(IShoppingCartService shoppingCartService)
            {
                _shoppingCartService = shoppingCartService;
            }

            public async Task<ShoppingCart> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
            {
                return await _shoppingCartService.GetShoppingCartByOwnerAsync(cancellationToken);
            }
        }
    }
}
