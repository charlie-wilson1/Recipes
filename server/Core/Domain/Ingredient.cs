namespace Recipes.Core.Domain
{
    public class Ingredient
    {
        public Ingredient() { }

        public Ingredient(
            string name,
            string notes,
            int quantity,
            int orderNumber)
        {
            Name = name;
            Notes = notes;
            Quantity = quantity;
            OrderNumber = orderNumber;
        }

        public string Name { get; protected set; }
        public string Unit { get; protected set; }
        public string Notes { get; protected set; }
        public int Quantity { get; protected set; }
        public int OrderNumber { get; protected set; }

        public void Upsert(
            string name,
            string unit,
            string notes,
            int quantity,
            int orderNumber)
        {
            Name = name;
            Unit = unit;
            Notes = notes;
            Quantity = quantity;
            OrderNumber = orderNumber;
        }
    }
}
