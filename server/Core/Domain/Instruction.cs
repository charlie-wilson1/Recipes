namespace Recipes.Core.Domain
{
    public class Instruction
    {
        public Instruction() { }

        public Instruction(
            int orderNumber,
            string description)
        {
            OrderNumber = orderNumber;
            Description = description;
        }

        public int OrderNumber { get; protected set; }
        public string Description { get; protected set; }

        public void Upsert(int orderNumber, string description)
        {
            OrderNumber = orderNumber;
            Description = description;
        }
    }
}
