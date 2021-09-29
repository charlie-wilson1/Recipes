using AutoMapper;
using MediatR;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Application.ShoppingCarts.Dtos;
using Recipes.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.ShoppingCarts.Commands
{
    public class AddItemToShoppingCartCommand : IRequest<ShoppingCart>
    {
        public AddShoppingCartItemRequest Item { get; set; }

        public class Handler : IRequestHandler<AddItemToShoppingCartCommand, ShoppingCart>
        {
            private readonly IMapper _mapper;
            private readonly IShoppingCartService _shoppingCartService;
            private readonly IShoppingCartRepository _shoppingCartRepository;

            public Handler(IMapper mapper, IShoppingCartService shoppingCartService, IShoppingCartRepository shoppingCartRepository)
            {
                _mapper = mapper;
                _shoppingCartService = shoppingCartService;
                _shoppingCartRepository = shoppingCartRepository;
            }

            public async Task<ShoppingCart> Handle(AddItemToShoppingCartCommand request, CancellationToken cancellationToken)
            {
                var shoppingCart = await _shoppingCartService.GetShoppingCartByOwnerAsync(cancellationToken);
                var item = _mapper.Map<ShoppingCartItem>(request.Item);
                shoppingCart.AddItem(item);
                await _shoppingCartRepository.UpdateAsync(shoppingCart, cancellationToken);
                return shoppingCart;
            }
        }
    }
}
