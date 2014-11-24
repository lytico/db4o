using Db4objects.Db4o.Config.Attributes;

namespace Db4oDoc.Code.Performance
{
    public class ItemHolder
    {
        [Indexed] private readonly Item indexedReference;

        public ItemHolder(Item item)
        {
            this.indexedReference = item;
        }

        public static ItemHolder Create(Item reference)
        {
            return new ItemHolder(reference);
        }

        public Item IndexedReference
        {
            get { return indexedReference; }
        }
    }
}