using FluentValidation;
using Recipes.Application.Common.Validators;
using Recipes.Application.Dtos.Recipes.Commands;
using Recipes.Application.Dtos.Recipes.UpsertDtos;

namespace Recipes.Application.Dtos.Recipes.Validators
{
    public class UpdateRecipeCommandValidator : AbstractValidator<UpdateRecipeCommand>
    {
        public UpdateRecipeCommandValidator()
        {
            RuleFor(x => x.CookTime).GreaterThan(0);
            RuleFor(x => x.PrepTime).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty();
            
            RuleFor(x => x.Id)
                .NotNull()
                .GreaterThan(default(int));

            RuleFor(x => x.Instructions).SetValidator(new MinListCountValidator<UpdateInstruction>(1));
            RuleFor(x => x.Ingredients).SetValidator(new MinListCountValidator<UpdateIngredient>(1));
        }
    }
}
