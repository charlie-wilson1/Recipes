using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities.Recipes;

namespace Recipes.Infrastructure.Persistence.EntityTypeConfigurations.Recipes
{
    public class RecipeImageTypeConfiguration : IEntityTypeConfiguration<RecipeImage>
    {
        public void Configure(EntityTypeBuilder<RecipeImage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Url).IsRequired();
            builder.Property(x => x.Name).IsRequired();
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
