/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when an old file format was detected
	/// and
	/// <see cref="Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates(bool)">Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates(bool)
	/// 	</see>
	/// is set to false.
	/// </summary>
	[System.Serializable]
	public class OldFormatException : Db4oFatalException
	{
		/// <summary>Constructor with the default message.</summary>
		/// <remarks>Constructor with the default message.</remarks>
		public OldFormatException() : base(Db4objects.Db4o.Internal.Messages.OldDatabaseFormat
			)
		{
		}
	}
}
