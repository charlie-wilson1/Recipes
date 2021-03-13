using System.Collections.Generic;

namespace Recipes.Application.Dtos.Recipes.UpsertDtos
{
    public class UpdateIngredient
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int UnitId { get; set; }
        public int Quantity { get; set; }
        public int OrderNumber { get; set; }

    }
}
