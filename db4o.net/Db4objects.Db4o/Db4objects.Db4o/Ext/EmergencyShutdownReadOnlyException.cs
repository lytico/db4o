/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// A previous IO exception has switched the database file
	/// to read-only mode for controlled shutdown.
	/// </summary>
	/// <remarks>
	/// A previous IO exception has switched the database file
	/// to read-only mode for controlled shutdown.
	/// </remarks>
	[System.Serializable]
	public class EmergencyShutdownReadOnlyException : Db4oIOException
	{
	}
}
