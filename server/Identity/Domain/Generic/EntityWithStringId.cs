namespace Recipes.Identity.Domain.Generic
{
    public abstract class EntityWithStringId
    {
        public string Id { get; protected set; }
    }
}
