
using System;
using System.Collections.Generic;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Linq.Caching
{
	/// <summary>
	/// Generic wrappers for <see cref="ICache4"/>.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class CacheFactory<TKey, TValue>
	{
		public static ICache4<TKey, TValue> For(ICache4 cache4)
		{
			return SynchonizedCache(new Cache4CachingStrategy<TKey, TValue>(cache4));
		}

		public static ICache4<TKey, TValue> For(ICache4 cache4, IEqualityComparer<TKey> comparer)
		{
			return SynchonizedCache(new Cache4CachingStrategyWithComparer<TKey, TValue>(cache4, comparer));
		}

        private static ICache4<TKey, TValue> SynchonizedCache(ICache4<TKey, TValue> cacheImplementation)
        {
            return new SynchronizedCache<TKey, TValue>(cacheImplementation);
        }
	}

	internal class Cache4CachingStrategy<TKey, TValue> : ICache4<TKey, TValue>
	{
		private readonly ICache4 _cache4;

		public Cache4CachingStrategy(ICache4 cache4)
		{
			_cache4 = cache4;
		}

		virtual public TValue Produce(TKey key, Func<TKey, TValue> producer)
		{
			return (TValue) Cache4Produce(key, producer);
		}

		virtual protected object Cache4Produce(object key, Func<TKey, TValue> producer)
		{
			return Cache4Produce(key, new Function4Func<TKey, TValue>(producer));
		}

		virtual protected object Cache4Produce(object key, IFunction4 producer)
		{
			return _cache4.Produce(key, producer, null);
		}
	}

	internal class Cache4CachingStrategyWithComparer<TKey, TValue> : Cache4CachingStrategy<TKey, TValue>
	{
		private readonly IEqualityComparer<TKey> _comparer;

		public Cache4CachingStrategyWithComparer(ICache4 cache4, IEqualityComparer<TKey> comparer) : base(cache4)
		{
			_comparer = comparer;
		}

		public override TValue Produce(TKey key, Func<TKey, TValue> producer)
		{
			return (TValue) Cache4Produce(new ComparableKey(_comparer, key), new UnwrappingProducer(producer));
		}

		internal class UnwrappingProducer : IFunction4
		{
			private readonly Func<TKey, TValue> _producer;

			public UnwrappingProducer(Func<TKey, TValue> producer)
			{
				_producer = producer;
			}

			#region Implementation of IFunction4

			public object Apply(object arg)
			{
				return _producer(((ComparableKey) arg).Key);
			}

			#endregion
		}

		internal class ComparableKey
		{
			private readonly IEqualityComparer<TKey> _comparer;
			private readonly TKey _key;

			public ComparableKey(IEqualityComparer<TKey> comparer, TKey key)
			{
				_comparer = comparer;
				_key = key;
			}

			public TKey Key
			{
				get { return _key; }
			}

			public override bool Equals(object obj)
			{
				return _comparer.Equals(_key, ((ComparableKey) obj)._key);
			}

			public override int GetHashCode()
			{
				return _comparer.GetHashCode(_key);
			}
		}
	}

	internal class Function4Func<T, TResult> : IFunction4
	{
		private readonly Func<T, TResult> _func;

		public Function4Func(Func<T, TResult> func)
		{
			_func = func;
		}

		public object Apply(object arg)
		{
			return _func((T) arg);
		}
	}
}
