using System;
using System.Diagnostics;

#if !SILVERLIGHT

namespace Db4objects.Db4o.Linq.Caching
{
	public class TracingCacheDecorator<TKey, TValue> : ICache4<TKey, TValue>
	{
		private readonly ICache4<TKey, TValue> _delegate;

		public TracingCacheDecorator(ICache4<TKey, TValue> @delegate)
		{
			_delegate = @delegate;
		}

		public TValue Produce(TKey key, Func<TKey, TValue> producer)
		{
			var hit = true;
			var result = _delegate.Produce(key, delegate(TKey newKey)
			                                    {
			                                    	hit = false;
			                                    	return producer(newKey);
			                                    });
			TraceCacheHitMiss(key, hit);
			return result;
		}

		private void TraceCacheHitMiss(TKey key, bool hit)
		{
			if (hit)
				Trace.WriteLine("Cache hit: " + key);
			else
				Trace.WriteLine("Cache miss: " + key);
		}
	}
}

#endif
