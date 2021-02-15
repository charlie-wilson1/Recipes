using Recipes.Domain.Entities.Generic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipes.Application.Contracts.RecipeRepositories
{
    public interface IGenericAuditableEntityRepository<TEntity> where TEntity : AuditableEntity
    {
        Task DeleteAuditableEntities(IEnumerable<TEntity> entities, bool saveEntities = true);
    }
}
