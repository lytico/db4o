/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Config
{
	/// <exclude></exclude>
	public class LegacyClientServerFactoryImpl : ILegacyClientServerFactory
	{
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.InvalidPasswordException"></exception>
		public virtual IObjectContainer OpenClient(IConfiguration config, string hostName
			, int port, string user, string password)
		{
			if (user == null || password == null)
			{
				throw new InvalidPasswordException();
			}
			Config4Impl.AssertIsNotTainted(config);
			IClientConfiguration clientConfig = Db4oClientServerLegacyConfigurationBridge.AsClientConfiguration
				(config);
			Socket4Adapter networkSocket = new Socket4Adapter(clientConfig.Networking.SocketFactory
				, hostName, port);
			return new ClientObjectContainer(clientConfig, networkSocket, user, password, true
				);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.IncompatibleFileFormatException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseFileLockedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public virtual IObjectServer OpenServer(IConfiguration config, string databaseFileName
			, int port)
		{
			LocalObjectContainer container = (LocalObjectContainer)Db4oFactory.OpenFile(config
				, databaseFileName);
			if (container == null)
			{
				return null;
			}
			IServerConfiguration serverConfig = Db4oClientServerLegacyConfigurationBridge.AsServerConfiguration
				(config);
			lock (container.Lock())
			{
				return new ObjectServerImpl(container, serverConfig, port);
			}
		}
	}
}
