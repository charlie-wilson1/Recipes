using FluentValidation;
using Recipes.Core.Application.Recipes.Commands;

namespace Recipes.Core.Application.Recipes.Validators
{
    public class DeleteRecipeCommandValidator : AbstractValidator<DeleteRecipeCommand>
    {
        public DeleteRecipeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
