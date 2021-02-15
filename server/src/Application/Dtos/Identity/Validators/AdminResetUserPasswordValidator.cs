using FluentValidation;
using Recipes.Application.Dtos.Identity.Commands;

namespace Recipes.Application.Dtos.Identity.Validators
{
    public class AdminResetUserPasswordValidator : AbstractValidator<AdminResetUserPasswordCommand>
    {
        public AdminResetUserPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
