using System.Text.RegularExpressions;
using FluentValidation;
using Recipes.Identity.Application.Identity.Commands;

namespace Recipes.Identity.Application.Identity.Validators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Password).PasswordValidator();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Username).NotEmpty();

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Password and confirmation must be the same.");
        }
    }
}
