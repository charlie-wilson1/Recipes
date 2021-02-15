using FluentValidation;
using Recipes.Application.Dtos.Identity.Commands;

namespace Recipes.Application.Dtos.Identity.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
