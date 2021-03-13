using FluentValidation;
using Recipes.Application.Dtos.Recipes.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Recipes.Validators
{
    public class GetAllRecipesQueryValidator : AbstractValidator<GetAllRecipesQuery>
    {
        public GetAllRecipesQueryValidator()
        {
            RuleFor(x => x.Request.ResultsPerPage)
                .GreaterThan(default(int));
        }
    }
}
