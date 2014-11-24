/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>db4o-specific exception.</summary>
	/// <remarks>
	/// db4o-specific exception. <br /><br />
	/// This exception is thrown when the object container required for
	/// the current operation was closed or failed to open.
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Db4oFactory.OpenFile(string)">Db4objects.Db4o.Db4oFactory.OpenFile(string)
	/// 	</seealso>
	/// <seealso cref="Db4objects.Db4o.IObjectContainer.Close()">Db4objects.Db4o.IObjectContainer.Close()
	/// 	</seealso>
	[System.Serializable]
	public class DatabaseClosedException : Db4oFatalException
	{
	}
}
