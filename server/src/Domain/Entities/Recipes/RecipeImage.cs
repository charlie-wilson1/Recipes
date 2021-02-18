using Recipes.Domain.Entities.Generic;

namespace Recipes.Domain.Entities.Recipes
{
    public class RecipeImage : AuditableEntity
    {
        public string Url { get; set; }
        public string Name { get; set; }
    }
}