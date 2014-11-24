/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.CS.Config;

namespace Db4objects.Db4o.Tests.Common.CS.Config
{
	public class ClientConfigurationItemIntegrationTestCase : ITestCase
	{
		private static readonly string Password = "db4o";

		private static readonly string User = "db4o";

		public virtual void Test()
		{
			IServerConfiguration serverConfig = Db4oClientServer.NewServerConfiguration();
			serverConfig.File.Storage = new MemoryStorage();
			IObjectServer server = Db4oClientServer.OpenServer(serverConfig, string.Empty, Db4oClientServer
				.ArbitraryPort);
			server.GrantAccess(User, Password);
			IClientConfiguration clientConfig = Db4oClientServer.NewClientConfiguration();
			ClientConfigurationItemIntegrationTestCase.DummyConfigurationItem item = new ClientConfigurationItemIntegrationTestCase.DummyConfigurationItem
				(this);
			clientConfig.AddConfigurationItem(item);
			IExtClient client = (IExtClient)Db4oClientServer.OpenClient(clientConfig, "localhost"
				, server.Ext().Port(), User, Password);
			item.Verify(clientConfig, client);
			client.Close();
			server.Close();
		}

		private sealed class DummyConfigurationItem : IClientConfigurationItem
		{
			private int _prepareCount = 0;

			private int _applyCount = 0;

			private IClientConfiguration _config;

			private IExtClient _client;

			public void Prepare(IClientConfiguration configuration)
			{
				this._config = configuration;
				this._prepareCount++;
			}

			public void Apply(IExtClient client)
			{
				this._client = client;
				this._applyCount++;
			}

			internal void Verify(IClientConfiguration config, IExtClient client)
			{
				Assert.AreSame(config, this._config);
				Assert.AreSame(client, this._client);
				Assert.AreEqual(1, this._prepareCount);
				Assert.AreEqual(1, this._applyCount);
			}

			internal DummyConfigurationItem(ClientConfigurationItemIntegrationTestCase _enclosing
				)
			{
				this._enclosing = _enclosing;
			}

			private readonly ClientConfigurationItemIntegrationTestCase _enclosing;
		}
	}
}
#endif // !SILVERLIGHT
