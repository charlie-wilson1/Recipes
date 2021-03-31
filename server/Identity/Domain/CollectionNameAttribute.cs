using System;

namespace Recipes.Identity.Domain
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CollectionNameAttribute : Attribute
    {
        public string Name { get; }

        public CollectionNameAttribute(string name) => Name = name;
    }
}
