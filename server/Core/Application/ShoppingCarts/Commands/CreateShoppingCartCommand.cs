using MediatR;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Domain;
using System;
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
                var existingCart = await _shoppingCartRepository.GetByOwnerAsync(_currentUserService.Username, cancellationToken);

                if (existingCart != null)
                {
                    return existingCart;
                }

                var cart = new ShoppingCart(_currentUserService.Username);
                await _shoppingCartRepository.InsertAsync(cart, cancellationToken);
                return cart;
            }
        }
    }
}
