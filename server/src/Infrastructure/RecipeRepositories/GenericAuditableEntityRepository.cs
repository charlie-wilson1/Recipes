using Recipes.Application.Contracts.RecipeRepositories;
using Recipes.Domain.Entities.Generic;
using Recipes.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.RecipeRepositories
{
    public class GenericAuditableEntityRepository<TEntity> : IGenericAuditableEntityRepository<TEntity>
        where TEntity : AuditableEntity
    {
        private readonly ApplicationDbContext _context;

        public GenericAuditableEntityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAuditableEntities(IEnumerable<TEntity> entities, bool saveEntities = true)
        {
            if (!entities.Any())
            {
                return;
            }

            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }

            _context.Set<TEntity>().UpdateRange(entities);

            if (saveEntities)
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
