using System;

namespace Db4objects.Db4o.Linq.Caching
{
    class SynchronizedCache<TKey, TValue> : ICache4<TKey, TValue>
    {
        private readonly object sync = new object();
        private readonly ICache4<TKey, TValue> implementation;

        public SynchronizedCache(ICache4<TKey, TValue> implementation)
        {
            if(null==implementation)
            {
                throw new ArgumentNullException("implementation");
            }
            this.implementation = implementation;
        }

        public TValue Produce(TKey key, Func<TKey, TValue> producer)
        {
            lock (sync)
            {
                return implementation.Produce(key, producer);    
            }
        }
    }
}