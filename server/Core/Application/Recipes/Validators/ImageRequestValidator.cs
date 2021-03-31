using FluentValidation;
using Recipes.Core.Application.Recipes.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Core.Application.Recipes.Validators
{
    public class ImageRequestValidator : AbstractValidator<ImageRequest>
    {
        public ImageRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Url).NotEmpty();
        }
    }
}
