using MediatR;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.ShoppingCarts.Commands
{
    public class CreateShoppingCartCommand : IRequest<ShoppingCart>
    {
        public class Handler : IRequestHandler<CreateShoppingCartCommand, ShoppingCart>
        {
            private readonly ICurrentUserService _currentUserService;
            private readonly IShoppingCartRepository _shoppingCartRepository;

            public Handler(ICurrentUserService currentUserService, IShoppingCartRepository shoppingCartRepository)
            {
                _currentUserService = currentUserService;
                _shoppingCartRepository = shoppingCartRepository;
            }

            public async Task<ShoppingCart> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
            {
                var cart = new ShoppingCart(_currentUserService.Username);
                await _shoppingCartRepository.InsertAsync(cart, cancellationToken);
                return cart;
            }
        }
    }
}
