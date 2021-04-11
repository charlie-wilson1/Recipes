using FluentValidation;
using Recipes.Core.Application.Recipes.Commands;

namespace Recipes.Core.Application.Recipes.Validators
{
    public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
    {
        public UploadImageCommandValidator()
        {
            this.CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Image).NotNull();

            this.CascadeMode = CascadeMode.Continue;
            RuleFor(x => x.Image.FileBytes).NotEmpty();
            RuleFor(x => x.Image.FileName).NotEmpty();
            RuleFor(x => x.Image.FileType).NotEmpty();
        }
    }
}
