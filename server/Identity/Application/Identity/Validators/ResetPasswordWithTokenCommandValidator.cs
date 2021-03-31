using FluentValidation;
using Recipes.Identity.Application.Identity.Commands;

namespace Recipes.Identity.Application.Identity.Validators
{
    public class ResetPasswordWithTokenCommandValidator : AbstractValidator<ResetPasswordWithTokenCommand>
    {
        public ResetPasswordWithTokenCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.Password).PasswordValidator();
            RuleFor(x => x.PasswordConfirm).Matches(x => x.Password);
        }
    }
}
