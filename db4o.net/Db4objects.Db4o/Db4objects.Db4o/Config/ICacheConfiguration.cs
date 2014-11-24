/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Config
{
	/// <summary>Interface to configure the cache configurations.</summary>
	/// <remarks>Interface to configure the cache configurations.</remarks>
	public interface ICacheConfiguration
	{
		/// <summary>
		/// configures the size of the slot cache to hold a number of
		/// slots in the cache.
		/// </summary>
		/// <remarks>
		/// configures the size of the slot cache to hold a number of
		/// slots in the cache.
		/// </remarks>
		/// <value>the number of slots</value>
		[System.ObsoleteAttribute(@"since 7.14 BTrees have their own LRU cache now.")]
		int SlotCacheSize
		{
			set;
		}
	}
}
