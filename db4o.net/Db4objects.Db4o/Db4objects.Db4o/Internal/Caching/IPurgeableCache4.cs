/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Internal.Caching
{
	/// <exclude></exclude>
	public interface IPurgeableCache4 : ICache4
	{
		/// <summary>Removes the cached value with the specified key from this cache.</summary>
		/// <remarks>Removes the cached value with the specified key from this cache.</remarks>
		/// <param name="key"></param>
		/// <returns>the purged value or null</returns>
		object Purge(object key);
	}
}
