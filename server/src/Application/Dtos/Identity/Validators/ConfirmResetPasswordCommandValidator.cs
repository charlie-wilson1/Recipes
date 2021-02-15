using FluentValidation;
using Recipes.Application.Dtos.Identity.Commands;

namespace Recipes.Application.Dtos.Identity.Validators
{
    public class ConfirmResetPasswordCommandValidator : AbstractValidator<ConfirmResetPasswordCommand>
    {
        public ConfirmResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.ResetToken).NotEmpty();

            RuleFor(x => x.NewPassword).NotEmpty();

            RuleFor(x => x.NewPasswordConfirmation)
                .Equal(x => x.NewPassword)
                .WithMessage("Password and PasswordConfirmation must match");
        }
    }
}
