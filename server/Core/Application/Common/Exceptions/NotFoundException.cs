using System;

namespace Recipes.Core.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public NotFoundException(string name, string key)
            : base($"{name} with key \"{key}\" was not found") { }
    }
}
