using FluentValidation;
using Recipes.Application.Common.Validators;
using Recipes.Application.Dtos.Identity.Commands;

namespace Recipes.Application.Dtos.Identity.Validators
{
    public class UpdateRolesCommandValidator : AbstractValidator<UpdateRolesCommand>
    {
        public UpdateRolesCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Roles).SetValidator(new MinListCountValidator<string>(1));
        }
    }
}
