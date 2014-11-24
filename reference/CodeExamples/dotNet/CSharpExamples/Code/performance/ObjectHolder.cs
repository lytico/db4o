using Db4objects.Db4o.Config.Attributes;

namespace Db4oDoc.Code.Performance
{
    public class ObjectHolder
    {
        [Indexed] private readonly object indexedReference;

        public ObjectHolder(object item)
        {
            this.indexedReference = item;
        }

        public static ObjectHolder Create(object reference)
        {
            return new ObjectHolder(reference);
        }

        public object IndexedReference
        {
            get { return indexedReference; }
        }
    }
}