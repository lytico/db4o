/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Config
{
	/// <exclude></exclude>
	public class CacheConfigurationImpl : ICacheConfiguration
	{
		private readonly Config4Impl _config;

		public CacheConfigurationImpl(Config4Impl config)
		{
			_config = config;
		}

		[System.ObsoleteAttribute(@"since 7.14 BTrees have their own LRU cache now.")]
		public virtual int SlotCacheSize
		{
			set
			{
				int size = value;
			}
		}
	}
}
