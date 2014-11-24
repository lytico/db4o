/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.CS.Config;

namespace Db4objects.Db4o.CS.Config
{
	/// <summary>factory to open C/S server and client implementations.</summary>
	/// <remarks>factory to open C/S server and client implementations.</remarks>
	/// <seealso cref="Db4objects.Db4o.CS.Db4oClientServer.OpenClient(IClientConfiguration, string, int, string, string)
	/// 	">Db4objects.Db4o.CS.Db4oClientServer.OpenClient(IClientConfiguration, string, int, string, string)
	/// 	</seealso>
	/// <seealso cref="Db4objects.Db4o.CS.Db4oClientServer.OpenServer(IServerConfiguration, string, int)
	/// 	">Db4objects.Db4o.CS.Db4oClientServer.OpenServer(IServerConfiguration, string, int)
	/// 	</seealso>
	public interface IClientServerFactory
	{
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.InvalidPasswordException"></exception>
		IObjectContainer OpenClient(IClientConfiguration config, string hostName, int port
			, string user, string password);

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.IncompatibleFileFormatException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseFileLockedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		IObjectServer OpenServer(IServerConfiguration config, string databaseFileName, int
			 port);
	}
}
