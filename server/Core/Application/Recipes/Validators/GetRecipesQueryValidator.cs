using FluentValidation;
using Recipes.Core.Application.Recipes.Queries;

namespace Recipes.Core.Application.Recipes.Validators
{
    public class GetRecipesQueryValidator : AbstractValidator<GetRecipesQuery>
    {
        public GetRecipesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .NotNull()
                .GreaterThan(0);
        }
    }
}
