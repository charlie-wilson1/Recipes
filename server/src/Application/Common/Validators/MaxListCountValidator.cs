using FluentValidation.Validators;
using System;
using System.Collections.Generic;

namespace Recipes.Application.Common.Validators
{
	public class MaxListCountValidator<T> : PropertyValidator
	{
		private int _max;

		public MaxListCountValidator(int max)
		{
			_max = max;
		}

		protected override bool IsValid(PropertyValidatorContext context)
		{
			var list = context.PropertyValue as IList<T>;

			if (list != null && list.Count >= _max)
			{
				context.MessageFormatter.AppendArgument("MaxElements", _max);
				return false;
			}

			return true;
		}

		protected override string GetDefaultMessageTemplate()
			=> "{PropertyName} must contain fewer than {MaxElements} items.";
	}
}
