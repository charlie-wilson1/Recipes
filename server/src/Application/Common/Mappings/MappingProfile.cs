using AutoMapper;
using Recipes.Application.Dtos.Recipes.Responses;
using Recipes.Domain.Entities.Recipes;

namespace Recipes.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // entity => response
            CreateMap<Recipe, RecipeResponse>()
                .ForMember(destination => destination.ImageName,
                           opt => opt.MapFrom(
                               source => source.Image != null ?
                                source.Image.Name :
                                string.Empty))
                .ForMember(destination => destination.ImageUrl,
                           opt => opt.MapFrom(
                               source => source.Image != null ?
                                source.Image.Url :
                                string.Empty))
                .ForMember(destination => destination.TotalTime,
                           opt => opt.MapFrom(
                               source => source.PrepTime + source.CookTime));

            CreateMap<Ingredient, IngredientDto>();
            CreateMap<Instruction, InstructionDto>();
            CreateMap<RecipeNote, RecipeNoteDto>();
            CreateMap<RecipeImage, ImageDto>();
        }
    }
}
