using FluentValidation;
using Recipes.Identity.Application.Identity.Commands;

namespace Recipes.Identity.Application.Identity.Validators
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
