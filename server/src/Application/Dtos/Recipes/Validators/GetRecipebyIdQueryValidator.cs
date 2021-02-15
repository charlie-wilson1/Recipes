using FluentValidation;
using Recipes.Application.Dtos.Recipes.Queries;

namespace Recipes.Application.Dtos.Recipes.Validators
{
    public class GetRecipeByIdQueryValidator : AbstractValidator<GetRecipeByIdQuery>
    {
        public GetRecipeByIdQueryValidator()
        {
            RuleFor(x => x.RecipeId)
                .NotNull()
                .GreaterThan(default(int));
        }
    }
}
