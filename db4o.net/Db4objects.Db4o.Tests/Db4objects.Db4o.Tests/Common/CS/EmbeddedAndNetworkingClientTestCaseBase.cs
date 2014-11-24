/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public abstract class EmbeddedAndNetworkingClientTestCaseBase : ITestLifeCycle
	{
		private static readonly string Username = "db4o";

		private static readonly string Password = "db4o";

		private IExtObjectServer _server;

		private IExtObjectContainer _networkingClient;

		private ObjectContainerSession _embeddedClient;

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			IServerConfiguration serverConfiguration = Db4oClientServer.NewServerConfiguration
				();
			serverConfiguration.File.Storage = new MemoryStorage();
			_server = Db4oClientServer.OpenServer(serverConfiguration, string.Empty, Db4oClientServer
				.ArbitraryPort).Ext();
			_server.GrantAccess(Username, Password);
			_networkingClient = Db4oClientServer.OpenClient("localhost", _server.Port(), Username
				, Password).Ext();
			this._embeddedClient = ((ObjectContainerSession)_server.OpenClient().Ext());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			EmbeddedClient().Close();
			NetworkingClient().Close();
			_server.Close();
		}

		protected virtual IExtObjectContainer NetworkingClient()
		{
			return _networkingClient;
		}

		protected virtual ObjectContainerSession EmbeddedClient()
		{
			return _embeddedClient;
		}

		protected virtual IExtObjectContainer ServerObjectContainer()
		{
			return _server.ObjectContainer().Ext();
		}
	}
}
#endif // !SILVERLIGHT
