/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;

namespace Db4objects.Db4o.Internal.Config
{
	public class Db4oLegacyConfigurationBridge
	{
		public static IEmbeddedConfiguration AsEmbeddedConfiguration(IConfiguration legacy
			)
		{
			return new EmbeddedConfigurationImpl(legacy);
		}

		public static ICommonConfiguration AsCommonConfiguration(IConfiguration config)
		{
			return new CommonConfigurationImpl((Config4Impl)config);
		}

		public static Config4Impl AsLegacy(object config)
		{
			return ((ILegacyConfigurationProvider)config).Legacy();
		}

		public static IFileConfiguration AsFileConfiguration(IConfiguration config)
		{
			return new FileConfigurationImpl((Config4Impl)config);
		}

		public static IIdSystemConfiguration AsIdSystemConfiguration(IConfiguration config
			)
		{
			return new IdSystemConfigurationImpl((Config4Impl)config);
		}
	}
}
