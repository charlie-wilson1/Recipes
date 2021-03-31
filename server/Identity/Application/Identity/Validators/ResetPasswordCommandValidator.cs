using FluentValidation;
using Recipes.Identity.Application.Identity.Commands;

namespace Recipes.Identity.Application.Identity.Validators
{
    public class ResetPasswordCommandValidator :  AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword).PasswordValidator();
            RuleFor(x => x.NewPasswordConfirmation).Matches(x => x.NewPassword);
        }
    }
}
