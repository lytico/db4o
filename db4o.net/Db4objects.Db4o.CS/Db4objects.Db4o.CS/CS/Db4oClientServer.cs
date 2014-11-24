/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS
{
	/// <summary>
	/// Factory class to open db4o servers and to connect db4o clients
	/// to them.
	/// </summary>
	/// <remarks>
	/// Factory class to open db4o servers and to connect db4o clients
	/// to them.
	/// <br /><br />
	/// <b>Note:<br />
	/// This class is made available in db4o-X.x-cs-java.jar / Db4objects.Db4o.CS.dll</b>
	/// </remarks>
	/// <since>7.5</since>
	public class Db4oClientServer
	{
		public const int ArbitraryPort = -1;

		/// <summary>
		/// creates a new
		/// <see cref="Db4objects.Db4o.CS.Config.IServerConfiguration">Db4objects.Db4o.CS.Config.IServerConfiguration
		/// 	</see>
		/// </summary>
		public static IServerConfiguration NewServerConfiguration()
		{
			return new ServerConfigurationImpl(NewLegacyConfig());
		}

		/// <summary>
		/// opens an
		/// <see cref="Db4objects.Db4o.IObjectServer">IObjectServer</see>
		/// on the specified database file and port.
		/// <br /><br />
		/// </summary>
		/// <param name="config">
		/// a custom
		/// <see cref="Db4objects.Db4o.CS.Config.IServerConfiguration">Db4objects.Db4o.CS.Config.IServerConfiguration
		/// 	</see>
		/// instance to be obtained via
		/// <see cref="NewServerConfiguration()">NewServerConfiguration()</see>
		/// </param>
		/// <param name="databaseFileName">an absolute or relative path to the database file</param>
		/// <param name="port">
		/// the port to be used or 0 if the server should not open a port, specify a value &lt; 0 if an arbitrary free port should be chosen - see
		/// <see cref="Db4objects.Db4o.Ext.IExtObjectServer.Port()">Db4objects.Db4o.Ext.IExtObjectServer.Port()
		/// 	</see>
		/// .
		/// </param>
		/// <returns>
		/// an
		/// <see cref="Db4objects.Db4o.IObjectServer">IObjectServer</see>
		/// listening
		/// on the specified port.
		/// </returns>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException">I/O operation failed or was unexpectedly interrupted.
		/// 	</exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseFileLockedException">
		/// the required database file is locked by
		/// another process.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.IncompatibleFileFormatException">runtime</exception>
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException">
		/// open operation failed because the database file
		/// is in old format and
		/// <see cref="Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates(bool)">Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates(bool)
		/// 	</see>
		/// 
		/// is set to false.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException">database was configured as read-only.
		/// 	</exception>
		public static IObjectServer OpenServer(IServerConfiguration config, string databaseFileName
			, int port)
		{
			return config.Networking.ClientServerFactory.OpenServer(config, databaseFileName, 
				port);
		}

		/// <summary>opens a db4o server with a fresh server configuration.</summary>
		/// <remarks>opens a db4o server with a fresh server configuration.</remarks>
		/// <seealso cref="OpenServer(Db4objects.Db4o.CS.Config.IServerConfiguration, string, int)
		/// 	">OpenServer(Db4objects.Db4o.CS.Config.IServerConfiguration, string, int)</seealso>
		/// <seealso cref="NewServerConfiguration()">NewServerConfiguration()</seealso>
		public static IObjectServer OpenServer(string databaseFileName, int port)
		{
			return OpenServer(NewServerConfiguration(), databaseFileName, port);
		}

		/// <summary>opens a db4o client instance with the specified configuration.</summary>
		/// <remarks>opens a db4o client instance with the specified configuration.</remarks>
		/// <param name="config">the configuration to be used</param>
		/// <param name="host">the host name of the server that is to be connected to</param>
		/// <param name="port">the server port to connect to</param>
		/// <param name="user">the username for authentication</param>
		/// <param name="password">the password for authentication</param>
		/// <seealso cref="OpenServer(Db4objects.Db4o.CS.Config.IServerConfiguration, string, int)
		/// 	">OpenServer(Db4objects.Db4o.CS.Config.IServerConfiguration, string, int)</seealso>
		/// <seealso cref="Db4objects.Db4o.IObjectServer.GrantAccess(string, string)">Db4objects.Db4o.IObjectServer.GrantAccess(string, string)
		/// 	</seealso>
		/// <exception cref="System.ArgumentException">if the configuration passed in has already been used.
		/// 	</exception>
		public static IObjectContainer OpenClient(IClientConfiguration config, string host
			, int port, string user, string password)
		{
			return config.Networking.ClientServerFactory.OpenClient(config, host, port, user, 
				password);
		}

		/// <summary>opens a db4o client instance with a fresh client configuration.</summary>
		/// <remarks>opens a db4o client instance with a fresh client configuration.</remarks>
		/// <seealso cref="OpenClient(Db4objects.Db4o.CS.Config.IClientConfiguration, string, int, string, string)
		/// 	">OpenClient(Db4objects.Db4o.CS.Config.IClientConfiguration, string, int, string, string)
		/// 	</seealso>
		/// <seealso cref="NewClientConfiguration()">NewClientConfiguration()</seealso>
		public static IObjectContainer OpenClient(string host, int port, string user, string
			 password)
		{
			return OpenClient(NewClientConfiguration(), host, port, user, password);
		}

		/// <summary>
		/// creates a new
		/// <see cref="Db4objects.Db4o.CS.Config.IClientConfiguration">Db4objects.Db4o.CS.Config.IClientConfiguration
		/// 	</see>
		/// 
		/// </summary>
		public static IClientConfiguration NewClientConfiguration()
		{
			Config4Impl legacy = NewLegacyConfig();
			return new ClientConfigurationImpl((Config4Impl)legacy);
		}

		private static Config4Impl NewLegacyConfig()
		{
			return (Config4Impl)Db4oFactory.NewConfiguration();
		}
	}
}
