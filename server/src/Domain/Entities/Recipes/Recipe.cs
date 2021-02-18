using Recipes.Domain.Entities.Generic;
using System.Collections.Generic;

namespace Recipes.Domain.Entities.Recipes
{
    public class Recipe : AuditableEntity
    {
        public string Name { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int? ImageId { get; set; }

        public virtual RecipeImage Image { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<Instruction> Instructions { get; set; }
        public virtual ICollection<RecipeNote> Notes { get; set; }
        public virtual ICollection<RecipeUser> SharedWithUsers { get; set; }
    }
}
