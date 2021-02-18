using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities.Recipes;

namespace Recipes.Infrastructure.Persistence.EntityTypeConfigurations.Recipes
{
    public class UnitTypeConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();

            builder.HasData(
                new Unit 
                {
                    Id = 1,
                    Name = "gm"
                },
                new Unit
                {
                    Id = 2,
                    Name = "cups"
                },
                new Unit
                {
                    Id = 3,
                    Name = "oz"
                },
                new Unit
                {
                    Id = 4,
                    Name = "tsp"
                },
                new Unit
                {
                    Id = 5,
                    Name = "tbs"
                },
                new Unit
                {
                    Id = 6,
                    Name = "ltr"
                },
                new Unit
                {
                    Id = 7,
                    Name = "qt"
                },
                new Unit
                {
                    Id = 8,
                    Name = "ml"
                },
                new Unit
                {
                    Id = 9,
                    Name = "pt"
                },
                new Unit
                {
                    Id = 10,
                    Name = "pinch"
                },
                new Unit
                {
                    Id = 11,
                    Name = "gal"
                },
                new Unit
                {
                    Id = 12,
                    Name = "lbs"
                },
                new Unit
                {
                    Id = 13,
                    Name = "kg"
                },
                new Unit
                {
                    Id = 43,
                    Name = "mg"
                },
                new Unit
                {
                    Id = 15,
                    Name = "mm"
                },
                new Unit
                {
                    Id = 16,
                    Name = "cm"
                },
                new Unit
                {
                    Id = 17,
                    Name = "in"
                },
                new Unit
                {
                    Id = 18,
                    Name = "F"
                });
        }
    }
}