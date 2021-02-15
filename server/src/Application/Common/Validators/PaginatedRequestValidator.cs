using FluentValidation;
using Recipes.Application.Dtos;

namespace Recipes.Application.Common.Validators
{
    public class PaginatedRequestValidator : AbstractValidator<PaginatedRequest>
    {
        public PaginatedRequestValidator()
        {
            RuleFor(x => x.ResultsPerPage)
                .GreaterThan(1);
        }
    }
}
