/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Config
{
	/// <exclude></exclude>
	public class IdSystemConfigurationImpl : IIdSystemConfiguration
	{
		private readonly Config4Impl _config;

		public IdSystemConfigurationImpl(Config4Impl config)
		{
			_config = config;
		}

		public virtual void UsePointerBasedSystem()
		{
			_config.UsePointerBasedIdSystem();
		}

		public virtual void UseStackedBTreeSystem()
		{
			_config.UseStackedBTreeIdSystem();
		}

		public virtual void UseInMemorySystem()
		{
			_config.UseInMemoryIdSystem();
		}

		public virtual void UseCustomSystem(IIdSystemFactory factory)
		{
			_config.UseCustomIdSystem(factory);
		}

		public virtual void UseSingleBTreeSystem()
		{
			_config.UseSingleBTreeIdSystem();
		}
	}
}
