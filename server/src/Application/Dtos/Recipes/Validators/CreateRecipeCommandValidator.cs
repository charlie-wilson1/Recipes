using FluentValidation;
using Recipes.Application.Common.Validators;
using Recipes.Application.Dtos.Recipes.Commands;
using Recipes.Application.Dtos.Recipes.UpsertDtos;

namespace Recipes.Application.Dtos.Recipes.Validators
{
    public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeCommandValidator()
        {
            RuleFor(x => x.CookTime).GreaterThan(0);
            RuleFor(x => x.PrepTime).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Instructions).SetValidator(new MinListCountValidator<CreateInstruction>(1));
            RuleFor(x => x.Ingredients).SetValidator(new MinListCountValidator<CreateIngredient>(1));
            RuleFor(x => x.Notes).SetValidator(new MinListCountValidator<CreateRecipeNote>(1));
        }
    }
}
