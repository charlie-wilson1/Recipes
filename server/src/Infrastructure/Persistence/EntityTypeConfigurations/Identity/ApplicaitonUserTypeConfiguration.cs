using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Infrastructure.Identity.Models;

namespace Recipes.Infrastructure.Persistence.EntityTypeConfigurations.Identity
{
    public class ApplicationUserTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasMany(x => x.Recipes)
                .WithOne()
                .HasForeignKey(y => y.CreatedByUserId)
                .IsRequired();

            builder.HasMany(x => x.SharedRecipes)
                .WithOne()
                .HasForeignKey(x => x.UserId)
                .IsRequired(false);
        }
    }
}
