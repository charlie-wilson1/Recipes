using FluentValidation;
using Recipes.Application.Dtos.Identity.Commands;

namespace Recipes.Application.Dtos.Identity.Validators
{
    public class UpdateCurrentUserCommandValidator : AbstractValidator<UpdateCurrentUserCommand>
    {
        public UpdateCurrentUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("{PropertyName} must be a valid email address")
                .NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.Username));

            RuleFor(x => x.Username)
                .NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.Email));
        }
    }
}
