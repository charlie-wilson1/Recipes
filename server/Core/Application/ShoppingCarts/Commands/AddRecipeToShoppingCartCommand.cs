using AutoMapper;
using MediatR;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.ShoppingCarts.Commands
{
    public class AddRecipeToShoppingCartCommand : IRequest<ShoppingCart>
    {
        public string RecipeId { get; set; }

        public class Handler : IRequestHandler<AddRecipeToShoppingCartCommand, ShoppingCart>
        {
            private readonly IMapper _mapper;
            private readonly IShoppingCartService _shoppingCartService;
            private readonly IRecipeRepository _recipeRepository;
            private readonly IShoppingCartRepository _shoppingCartRepository;

            public Handler(IMapper mapper, IShoppingCartService shoppingCartService, IRecipeRepository recipeRepository, IShoppingCartRepository shoppingCartRepository)
            {
                _mapper = mapper;
                _shoppingCartService = shoppingCartService;
                _recipeRepository = recipeRepository;
                _shoppingCartRepository = shoppingCartRepository;
            }

            public async Task<ShoppingCart> Handle(AddRecipeToShoppingCartCommand request, CancellationToken cancellationToken)
            {
                var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId, cancellationToken);
                var shoppingCart = await _shoppingCartService.GetShoppingCartByOwnerAsync(cancellationToken);
                shoppingCart.AddItems(_mapper.Map<List<ShoppingCartItem>>(recipe.Ingredients));
                await _shoppingCartRepository.UpdateAsync(shoppingCart, cancellationToken);
                return shoppingCart;
            }
        }
    }
}
