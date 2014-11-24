using System.Collections.Generic;
using System.Linq;

namespace Db4oDoc.Code.Performance
{
    public class CollectionHolder
    {
        private IList<Item> items;

        private CollectionHolder(IList<Item> items)
        {
            this.items = items;
        }

        public static CollectionHolder Create(params Item[] itemsToAdd)
        {
            return new CollectionHolder(itemsToAdd.ToList());
        }

        public IList<Item> Items
        {
            get { return items; }
        }
    }
}