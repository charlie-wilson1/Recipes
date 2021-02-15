using FluentValidation;
using Recipes.Application.Common.Validators;
using Recipes.Application.Dtos.Recipes.Commands;

namespace Recipes.Application.Dtos.Recipes.Validators
{
    public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
    {
        public UploadImageCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Url).SetValidator(new UrlValidator());
        }
    }
}
