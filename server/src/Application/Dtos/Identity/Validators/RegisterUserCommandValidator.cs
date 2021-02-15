using FluentValidation;
using Recipes.Application.Dtos.Identity.Commands;

namespace Recipes.Application.Dtos.Identity.Validators
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Username).NotEmpty();

            RuleFor(x => x.Password).NotEmpty();

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Password and PasswordConfirmation must match");
        }
    }
}
