/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o
{
	/// <summary>The db4o server interface.</summary>
	/// <remarks>
	/// The db4o server interface.
	/// <br /><br />- db4o servers can be opened with
	/// <see cref="Db4oClientServer#openServer(String,int)">Db4oClientServer#openServer(String,int)
	/// 	</see>
	/// .<br />
	/// - Direct in-memory connections to servers can be made with
	/// <see cref="OpenClient()">OpenClient()</see>
	/// <br />
	/// - TCP connections are available through
	/// <see cref="Db4oClientServer#openClient(String,int,String,String)">Db4oClientServer#openClient(String,int,String,String)
	/// 	</see>
	/// .
	/// <br /><br />Before connecting clients over TCP, you have to
	/// <see cref="GrantAccess(string, string)">GrantAccess(string, string)</see>
	/// to the username and password combination
	/// that you want to use.
	/// </remarks>
	/// <seealso cref="Db4oClientServer#openServer(java.lang.String,int)">Db4o.openServer
	/// 	</seealso>
	/// <seealso cref="Db4objects.Db4o.Ext.IExtObjectServer">ExtObjectServer for extended functionality
	/// 	</seealso>
	public interface IObjectServer : System.IDisposable
	{
		/// <summary>
		/// Closes the
		/// <see cref="IObjectServer"></see>
		/// and writes all cached data.
		/// <br /><br />
		/// </summary>
		/// <returns>
		/// true - denotes that the last instance connected to the
		/// used database file was closed.
		/// </returns>
		bool Close();

		/// <summary>
		/// Returns an
		/// <see cref="Db4objects.Db4o.Ext.IExtObjectServer">Db4objects.Db4o.Ext.IExtObjectServer
		/// 	</see>
		/// with extended functionality.
		/// <br /><br />Use this method to conveniently access extended methods.
		/// <br /><br />The functionality is split to two interfaces to allow newcomers to
		/// focus on the essential methods.
		/// </summary>
		IExtObjectServer Ext();

		/// <summary>Grants client access to the specified user with the specified password.</summary>
		/// <remarks>
		/// Grants client access to the specified user with the specified password.
		/// <br /><br />If the user already exists, the password is changed to
		/// the specified password.<br /><br />
		/// </remarks>
		/// <param name="userName">the name of the user</param>
		/// <param name="password">the password to be used</param>
		void GrantAccess(string userName, string password);

		/// <summary>Opens a client against this server.</summary>
		/// <remarks>
		/// Opens a client against this server.
		/// <br /><br />A client opened with this method operates within the same VM
		/// as the server. Since an embedded client use direct communication, without
		/// an in-between socket connection, performance will be better than a client
		/// opened with
		/// <see cref="Db4oClientServer#openClient(java.lang.String,int,java.lang.String,java.lang.String)
		/// 	">Db4oClientServer#openClient(java.lang.String,int,java.lang.String,java.lang.String)
		/// 	</see>
		/// <br /><br />Every client has it's own transaction and uses it's own cache
		/// for it's own version of all persistent objects.
		/// </remarks>
		IObjectContainer OpenClient();
	}
}
