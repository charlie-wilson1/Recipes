using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities.Recipes;

namespace Recipes.Infrastructure.Persistence.EntityTypeConfigurations.Recipes
{
    public class UnitTypeConfiguration : IEntityTypeConfiguration<UnitEntity>
    {
        public void Configure(EntityTypeBuilder<UnitEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();

            builder.HasData(
                new UnitEntity 
                {
                    Id = 1,
                    Name = "gm"
                },
                new UnitEntity
                {
                    Id = 2,
                    Name = "cups"
                },
                new UnitEntity
                {
                    Id = 3,
                    Name = "oz"
                },
                new UnitEntity
                {
                    Id = 4,
                    Name = "tsp"
                },
                new UnitEntity
                {
                    Id = 5,
                    Name = "tbs"
                },
                new UnitEntity
                {
                    Id = 6,
                    Name = "ltr"
                },
                new UnitEntity
                {
                    Id = 7,
                    Name = "qt"
                },
                new UnitEntity
                {
                    Id = 8,
                    Name = "ml"
                },
                new UnitEntity
                {
                    Id = 9,
                    Name = "pt"
                },
                new UnitEntity
                {
                    Id = 10,
                    Name = "pinch"
                },
                new UnitEntity
                {
                    Id = 11,
                    Name = "gal"
                },
                new UnitEntity
                {
                    Id = 12,
                    Name = "lbs"
                },
                new UnitEntity
                {
                    Id = 13,
                    Name = "kg"
                },
                new UnitEntity
                {
                    Id = 43,
                    Name = "mg"
                },
                new UnitEntity
                {
                    Id = 15,
                    Name = "mm"
                },
                new UnitEntity
                {
                    Id = 16,
                    Name = "cm"
                },
                new UnitEntity
                {
                    Id = 17,
                    Name = "in"
                },
                new UnitEntity
                {
                    Id = 18,
                    Name = "F"
                });
        }
    }
}