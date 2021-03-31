using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Core.Application.Recipes.Validators
{
    public static class CustomListValidators
    {
        public static IRuleBuilderOptions<T, List<K>> MinListLengthValidator<T,K>(this IRuleBuilder<T, List<K>> rule)
        {
            return rule
                .NotNull()
                .Must(list => list.Count > 0);
        }
    }
}
