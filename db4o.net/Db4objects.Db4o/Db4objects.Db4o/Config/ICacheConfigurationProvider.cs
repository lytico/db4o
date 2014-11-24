/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// A configuration provider that provides access
	/// to the cache-related configuration methods.
	/// </summary>
	/// <remarks>
	/// A configuration provider that provides access
	/// to the cache-related configuration methods.
	/// </remarks>
	public interface ICacheConfigurationProvider
	{
		/// <summary>Access to the cache-related configuration methods.</summary>
		/// <remarks>Access to the cache-related configuration methods.</remarks>
		ICacheConfiguration Cache
		{
			get;
		}
	}
}
