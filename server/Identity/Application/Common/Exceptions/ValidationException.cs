using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Recipes.Identity.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; } =
            new Dictionary<string, string[]>();

        public ValidationException()
            : base("One or more validation failures have occurred.") { }

        public ValidationException(string message) : base(message) { }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                Errors.Add(propertyName, propertyFailures);
            }
        }

        public ValidationException(IEnumerable<string> failures)
            : this()
        {
            var failureGroups = failures.GroupBy(e => e);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                Errors.Add(propertyName, propertyFailures);
            }
        }
    }
}
