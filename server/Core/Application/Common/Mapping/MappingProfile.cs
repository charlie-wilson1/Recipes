using AutoMapper;
using Recipes.Core.Application.Common.Models;
using Recipes.Core.Application.Recipes.Queries;

namespace Recipes.Core.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetRecipesQuery, PaginatedRequest>();
        }
    }
}
