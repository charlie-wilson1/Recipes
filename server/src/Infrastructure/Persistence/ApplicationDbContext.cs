using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;
using Recipes.Application.Contracts.Identity;
using Recipes.Domain.Entities.Generic;
using Recipes.Domain.Entities.Recipes;
using Recipes.Application.Contracts;

namespace Recipes.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
    {
        public DbSet<IngredientEntity> Ingredients { get; set; }
        public DbSet<InstructionEntity> Instructions { get; set; }
        public DbSet<RecipeEntity> Recipes { get; set; }
        public DbSet<RecipeImageEntity> RecipeImages { get; set; }
        public DbSet<RecipeNoteEntity> RecipeNotes { get; set; }
        public DbSet<RecipeUser> RecipesUsers { get; set; }
        public DbSet<UnitEntity> Units { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService, IDateTime dateTime)
            : base(options) 
        {
            UserId = currentUserService.UserId;
            _dateTime = dateTime;
        }

        private readonly string UserId;
        private IDateTime _dateTime;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedByUserId = UserId;
                        entry.Entity.CreatedDate = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedByUserId = UserId;
                        entry.Entity.LastModifiedDate = _dateTime.Now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
