using FluentValidation;
using Recipes.Core.Application.Recipes.Dtos.Requests;

namespace Recipes.Core.Application.Recipes.Validators
{
    public class InstructionRequestValidator : AbstractValidator<InstructionRequest>
    {
        public InstructionRequestValidator()
        {
            RuleFor(x => x.OrderNumber).NotNull().GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
