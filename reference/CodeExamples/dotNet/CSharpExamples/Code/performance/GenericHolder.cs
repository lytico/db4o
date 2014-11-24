using Db4objects.Db4o.Config.Attributes;

namespace Db4oDoc.Code.Performance
{

    public static class GenericHolder
    {
        public static GenericHolder<T> Create<T>(T reference)
        {
            return new GenericHolder<T>(reference);
        }
    }
    public class GenericHolder<T>
    {
        [Indexed]
        private readonly T indexedReference;

        public GenericHolder(T reference)
        {
            this.indexedReference = reference;
        }

        public T IndexedReference
        {
            get { return indexedReference; }
        }
    }
}