/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.IO
{
	/// <summary>
	/// Caching storage adapter to cache db4o database data in memory
	/// until the underlying
	/// <see cref="IBin">IBin</see>
	/// is instructed to flush its
	/// data when
	/// <see cref="IBin.Sync()">IBin.Sync()</see>
	/// is called.<br /><br />
	/// You can override the
	/// <see cref="NewCache()">NewCache()</see>
	/// method if you want to
	/// work with a different caching strategy.
	/// </summary>
	public class CachingStorage : StorageDecorator
	{
		private static int DefaultPageCount = 64;

		private static int DefaultPageSize = 1024;

		private int _pageCount;

		private int _pageSize;

		/// <summary>
		/// default constructor to create a Caching storage with the default
		/// page count of 64 and the default page size of 1024.
		/// </summary>
		/// <remarks>
		/// default constructor to create a Caching storage with the default
		/// page count of 64 and the default page size of 1024.
		/// </remarks>
		/// <param name="storage">
		/// the
		/// <see cref="IStorage">IStorage</see>
		/// to be cached.
		/// </param>
		public CachingStorage(IStorage storage) : this(storage, DefaultPageCount, DefaultPageSize
			)
		{
		}

		/// <summary>
		/// constructor to set up a CachingStorage with a configured page count
		/// and page size
		/// </summary>
		/// <param name="storage">
		/// the
		/// <see cref="IStorage">IStorage</see>
		/// to be cached.
		/// </param>
		/// <param name="pageCount">the number of pages the cache should use.</param>
		/// <param name="pageSize">the size of the pages the cache should use.</param>
		public CachingStorage(IStorage storage, int pageCount, int pageSize) : base(storage
			)
		{
			_pageCount = pageCount;
			_pageSize = pageSize;
		}

		/// <summary>opens a Bin for the given URI.</summary>
		/// <remarks>opens a Bin for the given URI.</remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override IBin Open(BinConfiguration config)
		{
			IBin storage = base.Open(config);
			if (config.ReadOnly())
			{
				return new ReadOnlyBin(new CachingStorage.NonFlushingCachingBin(storage, NewCache
					(), _pageCount, _pageSize));
			}
			return new CachingBin(storage, NewCache(), _pageCount, _pageSize);
		}

		/// <summary>
		/// override this method if you want to work with a different caching
		/// strategy than the default LRU2Q cache.
		/// </summary>
		/// <remarks>
		/// override this method if you want to work with a different caching
		/// strategy than the default LRU2Q cache.
		/// </remarks>
		protected virtual ICache4 NewCache()
		{
			return CacheFactory.NewLRULongCache(_pageCount);
		}

		private sealed class NonFlushingCachingBin : CachingBin
		{
			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public NonFlushingCachingBin(IBin bin, ICache4 cache, int pageCount, int pageSize
				) : base(bin, cache, pageCount, pageSize)
			{
			}

			protected override void FlushAllPages()
			{
			}
		}
	}
}
