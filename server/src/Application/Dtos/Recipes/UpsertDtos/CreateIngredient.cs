namespace Recipes.Application.Dtos.Recipes.UpsertDtos
{
    public class CreateIngredient
    {
        public string Name { get; set; }
        public int UnitId { get; set; }
        public string Notes { get; set; }
        public int Quantity { get; set; }
        public int OrderNumber { get; set; }
    }
}
