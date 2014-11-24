/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;

namespace Db4objects.Db4o.CS.Internal.Config
{
	public class Db4oClientServerLegacyConfigurationBridge
	{
		public static IClientConfiguration AsClientConfiguration(IConfiguration config)
		{
			return new ClientConfigurationImpl((Config4Impl)config);
		}

		public static IServerConfiguration AsServerConfiguration(IConfiguration config)
		{
			return new ServerConfigurationImpl((Config4Impl)config);
		}

		public static Config4Impl AsLegacy(object config)
		{
			return ((ILegacyConfigurationProvider)config).Legacy();
		}

		public static INetworkingConfiguration AsNetworkingConfiguration(IConfiguration config
			)
		{
			return AsServerConfiguration(config).Networking;
		}
	}
}
