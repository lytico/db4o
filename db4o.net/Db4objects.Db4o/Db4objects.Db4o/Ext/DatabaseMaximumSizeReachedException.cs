/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when the database file reaches the
	/// maximum allowed size.
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when the database file reaches the
	/// maximum allowed size. Upon throwing the exception the database is
	/// switched to the read-only mode. <br />
	/// The maximum database size is configurable
	/// and can reach up to 254GB.
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.BlockSize(int)">Db4objects.Db4o.Config.IConfiguration.BlockSize(int)
	/// 	</seealso>
	[System.Serializable]
	public class DatabaseMaximumSizeReachedException : Db4oRecoverableException
	{
		public DatabaseMaximumSizeReachedException(int size) : base("Maximum database file size reached. Last valid size: "
			 + size + ". From now on opening only works in read-only mode.")
		{
		}
	}
}
