namespace Recipes.Core.Application.ShoppingCarts.Dtos
{
    public class ShoppingCartItemRequest
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
