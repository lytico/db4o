using Db4objects.Db4o.Internal.Caching;
using Db4objects.Db4o.IO;

namespace Db4oDoc.Code.Configuration.IO
{
    // #example: Exchange the cache-implementation
    public class LRUCachingStorage : CachingStorage {
        private readonly int pageCount;

        public LRUCachingStorage(IStorage storage):base(storage) {
            this.pageCount = 128;
        }

        public LRUCachingStorage(IStorage storage, int pageCount, int pageSize)
            : base(storage, pageCount, pageSize)
        {
            this.pageCount = pageCount;
        }

        protected override ICache4 NewCache()
        {
            return CacheFactory.NewLRUCache(pageCount);
        }
    }
    // #end example
}