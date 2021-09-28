namespace Recipes.Core.Domain
{
    public class ShoppingCartItem
    {
        public ShoppingCartItem(string name, int quantity, string unit)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
        }

        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public string Unit { get; private set; }

        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
        }
    }
}
