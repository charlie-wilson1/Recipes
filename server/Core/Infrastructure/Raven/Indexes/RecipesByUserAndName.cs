using Raven.Client.Documents.Indexes;
using Recipes.Core.Domain;
using System.Linq;

namespace Recipes.Core.Infrastructure.Raven.Indexes
{
    public class Recipes_ByUserAndName : AbstractIndexCreationTask<Recipe>
    {
        public Recipes_ByUserAndName()
        {
            Map = recipes => recipes
                .Select(recipe => new { recipe.Name, recipe.Owner, recipe.IsDeleted });
        }
    }
}
