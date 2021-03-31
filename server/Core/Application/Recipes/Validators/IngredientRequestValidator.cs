using System;
using System.Linq;
using FluentValidation;
using Recipes.Core.Application.Recipes.Dtos.Requests;

namespace Recipes.Core.Application.Recipes.Validators
{
    public class IngredientRequestValidator : AbstractValidator<IngredientRequest>
    {
        public IngredientRequestValidator()
        {
            RuleFor(x => x.Unit)
                .NotEmpty()
                .Must(x => Enum.GetNames(typeof(Units)).Contains(x))
                .WithMessage("Must be a valid unit.");

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.OrderNumber).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
        }
    }
}
