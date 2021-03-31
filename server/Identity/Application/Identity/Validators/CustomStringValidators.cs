using System.Text.RegularExpressions;
using FluentValidation;

namespace Recipes.Identity.Application.Identity.Validators
{
    public static class CustomStringValidators
    {
        public static IRuleBuilderOptions<T, string> PasswordValidator<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty()
                .MinimumLength(6)
                .Matches(new Regex("[a-z]"))
                .WithMessage("{PropertyName} must have a lowercase letter.")
                .Matches(new Regex("[A-Z]"))
                .WithMessage("{PropertyName} must have an uppercase letter.")
                .Matches(new Regex("[0-9]"))
                .WithMessage("{PropertyName} must have a number.")
                .Matches(new Regex("[@$!%*#?&]"))
                .WithMessage("{PropertyName} must have a special character.");
        }
    }
}
