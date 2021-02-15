using FluentValidation.Validators;

namespace Recipes.Application.Common.Validators
{
    public class UrlValidator : PropertyValidator
    {
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var stringQuery = context.PropertyValue as string;

            if (string.IsNullOrWhiteSpace(stringQuery) ||
                (!stringQuery.StartsWith("http://") ||
                !stringQuery.StartsWith("http://")) ||
                !stringQuery.Contains("."))
            {
                return false;
            }

            return true;
        }
    }
}
