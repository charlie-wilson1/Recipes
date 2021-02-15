using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities.Recipes;

namespace Recipes.Infrastructure.Persistence.EntityTypeConfigurations.Recipes
{
    public class RecipeTypeConfiguration : IEntityTypeConfiguration<RecipeEntity>
    {
        public void Configure(EntityTypeBuilder<RecipeEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.PrepTime).IsRequired();
            builder.Property(x => x.CookTime).IsRequired();
            builder.Property(x => x.ImageId).IsRequired(false);
            builder.Property(x => x.CreatedByUserId).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.LastModifiedByUserId).IsRequired(false);
            builder.Property(x => x.LastModifiedDate).IsRequired(false);
            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(x => x.Image)
                .WithMany()
                .HasForeignKey(y => y.ImageId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasMany(x => x.Notes)
                .WithOne(y => y.Recipe)
                .HasForeignKey(y => y.RecipeId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasMany(x => x.Instructions)
                .WithOne(y => y.Recipe)
                .HasForeignKey(y => y.RecipeId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasMany(x => x.Ingredients)
                .WithOne(y => y.Recipe)
                .HasForeignKey(y => y.RecipeId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasMany(x => x.SharedWithUsers)
                .WithOne(y => y.Recipe)
                .HasForeignKey(y => y.RecipeId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
