using FluentValidation;
using Recipes.Application.Dtos.Recipes.Commands;

namespace Recipes.Application.Dtos.Recipes.Validators
{
    public class DeleteRecipeCommandValidator : AbstractValidator<DeleteRecipeCommand>
    {
        public DeleteRecipeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .GreaterThan(default(int));
        }
    }
}
