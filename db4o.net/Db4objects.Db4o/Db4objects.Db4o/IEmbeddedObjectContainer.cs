/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o
{
	/// <summary>Represents a local ObjectContainer attached to a database file.</summary>
	/// <remarks>Represents a local ObjectContainer attached to a database file.</remarks>
	/// <since>7.10</since>
	public interface IEmbeddedObjectContainer : IObjectContainer
	{
		/// <summary>Backs up a database file of an open ObjectContainer.</summary>
		/// <remarks>
		/// Backs up a database file of an open ObjectContainer.
		/// <br /><br />
		/// While the backup is running, the ObjectContainer can continue to be
		/// used. Changes that are made while the backup is in progress will be applied to
		/// the object container and to the backup.<br /><br />
		/// While the backup is running, the object container should not be closed.<br /><br />
		/// If a file already exists at the specified path, it will be overwritten.<br /><br />
		/// The
		/// <see cref="Db4objects.Db4o.IO.IStorage">Db4objects.Db4o.IO.IStorage</see>
		/// used for backup is the one configured for this container. If you
		/// want to use another storage implementation for the backup please
		/// use
		/// <see cref="IObjectContainer.Ext()">ext()</see>
		/// .
		/// <see cref="Db4objects.Db4o.Ext.IExtObjectContainer.Backup(Db4objects.Db4o.IO.IStorage, string)
		/// 	">backup(com.db4o.io.Storage, String)</see>
		/// .
		/// </remarks>
		/// <param name="path">a the path to the backup file</param>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException">db4o database file was closed or failed to open.
		/// 	</exception>
		/// <exception cref="System.NotSupportedException">
		/// is thrown when the operation is not supported in current
		/// configuration/environment
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException">I/O operation failed or was unexpectedly interrupted.
		/// 	</exception>
		void Backup(string path);
	}
}
