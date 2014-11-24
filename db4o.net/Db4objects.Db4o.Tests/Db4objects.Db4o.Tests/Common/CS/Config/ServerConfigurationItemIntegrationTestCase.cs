/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.CS.Config;

namespace Db4objects.Db4o.Tests.Common.CS.Config
{
	public class ServerConfigurationItemIntegrationTestCase : ITestCase
	{
		public virtual void Test()
		{
			IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
			config.File.Storage = new MemoryStorage();
			ServerConfigurationItemIntegrationTestCase.DummyConfigurationItem item = new ServerConfigurationItemIntegrationTestCase.DummyConfigurationItem
				(this);
			config.AddConfigurationItem(item);
			IObjectServer server = Db4oClientServer.OpenServer(config, string.Empty, Db4oClientServer
				.ArbitraryPort);
			item.Verify(config, server);
			server.Close();
		}

		private sealed class DummyConfigurationItem : IServerConfigurationItem
		{
			private int _prepareCount = 0;

			private int _applyCount = 0;

			private IServerConfiguration _config;

			private IObjectServer _server;

			public void Prepare(IServerConfiguration configuration)
			{
				this._config = configuration;
				this._prepareCount++;
			}

			public void Apply(IObjectServer server)
			{
				this._server = server;
				this._applyCount++;
			}

			internal void Verify(IServerConfiguration config, IObjectServer server)
			{
				Assert.AreSame(config, this._config);
				Assert.AreSame(server, this._server);
				Assert.AreEqual(1, this._prepareCount);
				Assert.AreEqual(1, this._applyCount);
			}

			internal DummyConfigurationItem(ServerConfigurationItemIntegrationTestCase _enclosing
				)
			{
				this._enclosing = _enclosing;
			}

			private readonly ServerConfigurationItemIntegrationTestCase _enclosing;
		}
	}
}
#endif // !SILVERLIGHT
