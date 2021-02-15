using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities.Recipes;

namespace Recipes.Infrastructure.Persistence.EntityTypeConfigurations.Recipes
{
    public class RecipeUserTypeConfiguration : IEntityTypeConfiguration<RecipeUser>
    {
        public void Configure(EntityTypeBuilder<RecipeUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedByUserId).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.LastModifiedByUserId).IsRequired(false);
            builder.Property(x => x.LastModifiedDate).IsRequired(false);
            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
