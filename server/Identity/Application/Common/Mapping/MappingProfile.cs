using AutoMapper;
using Recipes.Identity.Application.Identity.Responses;
using Recipes.Identity.Domain;

namespace Recipes.Identity.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<ApplicationUser, UserWithTokenResponse>()
                .ForMember(x => x.Jwt, opt => opt.Ignore());
        }
    }
}
