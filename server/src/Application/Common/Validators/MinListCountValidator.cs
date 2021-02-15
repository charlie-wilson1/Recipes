using FluentValidation.Validators;
using System.Collections.Generic;

namespace Recipes.Application.Common.Validators
{
	public class MinListCountValidator<T> : PropertyValidator
	{
		private int _min;

		public MinListCountValidator(int min)
		{
			_min = min;
		}

		protected override bool IsValid(PropertyValidatorContext context)
		{
			var list = context.PropertyValue as IList<T>;

			if (list is null)
            {
				context.MessageFormatter.AppendArgument("MinElements", _min);
				return false;
			}

			if (list.Count < _min)
			{
				context.MessageFormatter.AppendArgument("MinElements", _min);
				return false;
			}

			return true;
		}

		protected override string GetDefaultMessageTemplate()
			=> "{PropertyName} must contain more than {MinElements} items.";
	}
}
