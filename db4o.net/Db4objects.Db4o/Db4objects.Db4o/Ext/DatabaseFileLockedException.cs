/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// db4o-specific exception.<br /><br />
	/// this Exception is thrown during any of the db4o open calls
	/// if the database file is locked by another process.
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br /><br />
	/// this Exception is thrown during any of the db4o open calls
	/// if the database file is locked by another process.
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Db4oFactory.OpenFile(string)">Db4objects.Db4o.Db4oFactory.OpenFile(string)
	/// 	</seealso>
	[System.Serializable]
	public class DatabaseFileLockedException : Db4oFatalException
	{
		/// <summary>Constructor with a database description message</summary>
		/// <param name="databaseDescription">message, which can help to identify the database
		/// 	</param>
		public DatabaseFileLockedException(string databaseDescription) : base(databaseDescription
			)
		{
		}

		/// <summary>Constructor with a database description and cause exception</summary>
		/// <param name="databaseDescription">database description</param>
		/// <param name="cause">previous exception caused DatabaseFileLockedException</param>
		public DatabaseFileLockedException(string databaseDescription, Exception cause) : 
			base(databaseDescription, cause)
		{
		}
	}
}
