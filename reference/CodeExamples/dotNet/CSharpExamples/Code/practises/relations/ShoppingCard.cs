using System.Collections.Generic;

namespace Db4oDoc.Code.Practises.Relations
{
    internal class ShoppingCard
    {
        // #example: Simple 1-n relation. Navigating from the card to the items
        private readonly ICollection<Item> items = new HashSet<Item>();

        public void Add(Item terrain)
        {
            items.Add(terrain);
        }

        public void Remove(Item o)
        {
            items.Remove(o);
        }
        // #end example
    }
}