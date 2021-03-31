using FluentValidation;
using Recipes.Core.Application.Recipes.Queries;

namespace Recipes.Core.Application.Recipes.Validators
{
    public class GetRecipesQueryValidator : AbstractValidator<GetRecipesQuery>
    {
        public GetRecipesQueryValidator()
        {
            RuleFor(x => x.Request.PageNumber)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.Request.PageSize)
                .NotNull()
                .GreaterThan(0);
        }
    }
}
