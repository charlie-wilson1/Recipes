using System;
using System.Collections.Generic;
using System.Linq;

namespace Recipes.Core.Domain
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }

        public ShoppingCart(string owner)
        {
            Owner = owner;
        }

        public string Id { get; private set; }
        public string Owner { get; private set; }

        public List<ShoppingCartItem> Items { get; set; }

        public void UpsertItems(List<ShoppingCartItem> items)
        {
            Items = items;
        }

        public void AddItem(ShoppingCartItem item)
        {
            Items.Add(item);
        }

        public void AddItems(List<ShoppingCartItem> items)
        {
            Items.AddRange(items);
        }

        public void RemoveItem(ShoppingCartItem item)
        {
            Items.Remove(item);
        }

        public ShoppingCartItem GetItemByName(string name)
        {
            return Items.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void EmptyCart()
        {
            Items.Clear();
        }
    }
}
