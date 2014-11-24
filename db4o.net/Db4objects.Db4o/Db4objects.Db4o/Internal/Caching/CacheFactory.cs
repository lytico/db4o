/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Internal.Caching
{
	/// <exclude></exclude>
	public class CacheFactory
	{
		public static ICache4 New2QCache(int size)
		{
			return new LRU2QCache(size);
		}

		public static ICache4 New2QLongCache(int size)
		{
			return new LRU2QLongCache(size);
		}

		public static ICache4 New2QXCache(int size)
		{
			return new LRU2QXCache(size);
		}

		public static IPurgeableCache4 NewLRUCache(int size)
		{
			return new LRUCache(size);
		}

		public static IPurgeableCache4 NewLRUIntCache(int size)
		{
			return new LRUIntCache(size);
		}

		public static IPurgeableCache4 NewLRULongCache(int size)
		{
			return new LRULongCache(size);
		}
	}
}
