using AutoMapper;
using Recipes.Core.Application.Common.Models;
using Recipes.Core.Application.Recipes.Queries;
using Recipes.Core.Application.ShoppingCarts.Dtos;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetRecipesQuery, PaginatedRequest>();
            CreateMap<AddShoppingCartItemRequest, ShoppingCartItem>();
            CreateMap<Ingredient, ShoppingCartItem>();
        }
    }
}
