/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Internal.Query
{
	public delegate R Producer<T, R>(T arg);

	/// <summary>
	/// A very simple caching strategy that caches only the last added item.
	/// </summary>
	public class SingleItemCachingStrategy<K,V> : ICachingStrategy<K,V>
	{

		private K _lastKey;
		private V _lastItem;
		private object _monitor = new object();
		private readonly Producer<K, V> _producer;
		
		public SingleItemCachingStrategy(Producer<K,V> producer)
		{
			_producer = producer;
		}

		#region ICachingStrategy Members
		public void Add(K key, V item)
		{
			if (!typeof(K).IsValueType && null == key) throw new ArgumentNullException("key");
			lock (_monitor)
			{
				_lastKey = key;
				_lastItem = item;
			}
		}

		public V Get(K key)
		{
			if (null == key) throw new ArgumentNullException("key");
			lock (_monitor)
			{
				if (!key.Equals(_lastKey))
				{
					Add(key, _producer(key));
				}
				
				return _lastItem;
			}
		}
		#endregion
	}
    
    public class NullCachingStrategy : ICachingStrategy<object, object>
    {
        public static readonly ICachingStrategy<object, object> Default = new NullCachingStrategy();
        
        #region ICachingStrategy Members
        public void Add(object key, object item)
        {   
        }

        public object Get(object key)
        {
            return null;
        }
        #endregion
    }
}