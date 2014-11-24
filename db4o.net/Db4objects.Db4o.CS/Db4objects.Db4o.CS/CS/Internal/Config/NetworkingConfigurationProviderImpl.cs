/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;

namespace Db4objects.Db4o.CS.Internal.Config
{
	public class NetworkingConfigurationProviderImpl : INetworkingConfigurationProvider
		, ILegacyConfigurationProvider
	{
		private readonly NetworkingConfigurationImpl _networking;

		public NetworkingConfigurationProviderImpl(Config4Impl config)
		{
			_networking = new NetworkingConfigurationImpl(config);
		}

		public virtual INetworkingConfiguration Networking
		{
			get
			{
				return _networking;
			}
		}

		public virtual Config4Impl Legacy()
		{
			return _networking.Config();
		}
	}
}
