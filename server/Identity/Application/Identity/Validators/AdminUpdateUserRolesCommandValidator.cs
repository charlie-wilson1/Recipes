using FluentValidation;
using Recipes.Identity.Application.Identity.Commands;
using System;
using System.Linq;

namespace Recipes.Identity.Application.Identity.Validators
{
    public class AdminUpdateUserRolesCommandValidator : AbstractValidator<AdminUpdateUserRolesCommand>
    {
        public AdminUpdateUserRolesCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Roles)
                .NotEmpty()
                .Must(x => x.Contains("Member"))
                .WithMessage("Must be at least a member.");

            RuleForEach(x => x.Roles)
                .Must(x => Enum.GetNames(typeof(Roles)).Contains(x))
                .WithMessage("Must be a valid role.");
        }
    }
}
