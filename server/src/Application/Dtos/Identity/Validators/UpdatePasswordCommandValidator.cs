using FluentValidation;
using Recipes.Application.Dtos.Identity.Commands;

namespace Recipes.Application.Dtos.Identity.Validators
{
    public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
    {
        public UpdatePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();

            RuleFor(x => x.NewPasswordConfirmation)
                .Equal(x => x.NewPassword)
                .WithMessage("Password and PasswordConfirmation must match");
        }
    }
}
