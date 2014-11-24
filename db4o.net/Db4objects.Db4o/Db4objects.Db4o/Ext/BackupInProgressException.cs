/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>db4o-specific exception.</summary>
	/// <remarks>
	/// db4o-specific exception. <br /><br />
	/// This exception is thrown when the current
	/// <see cref="IExtObjectContainer.Backup(string)">backup</see>
	/// process encounters another backup process already running.
	/// </remarks>
	[System.Serializable]
	public class BackupInProgressException : Db4oRecoverableException
	{
	}
}
