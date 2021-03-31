namespace Recipes.Core.Application.Recipes.Dtos.Requests
{
    public class IngredientRequest
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Notes { get; set; }
        public int Quantity { get; set; }
        public int OrderNumber { get; set; }
    }
}
