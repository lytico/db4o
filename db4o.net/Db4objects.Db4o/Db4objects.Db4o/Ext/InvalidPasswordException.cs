/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when a client tries to connect
	/// to a server with a wrong password or null password.
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when a client tries to connect
	/// to a server with a wrong password or null password.
	/// </remarks>
	[System.Serializable]
	public class InvalidPasswordException : Db4oRecoverableException
	{
	}
}
