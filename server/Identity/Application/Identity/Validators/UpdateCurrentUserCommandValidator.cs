using FluentValidation;
using Recipes.Identity.Application.Identity.Commands;

namespace Recipes.Identity.Application.Identity.Validators
{
    public class UpdateCurrentUserCommandValidator : AbstractValidator<UpdateCurrentUserCommand>
    {
        public UpdateCurrentUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
