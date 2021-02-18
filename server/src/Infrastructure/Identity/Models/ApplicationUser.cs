using Microsoft.AspNetCore.Identity;
using Recipes.Domain.Entities.Recipes;
using System.Collections.Generic;

namespace Recipes.Infrastructure.Identity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; }
        public virtual ICollection<RecipeUser> SharedRecipes { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<Recipe> ModifiedRecipes { get; set; }
    }
}
