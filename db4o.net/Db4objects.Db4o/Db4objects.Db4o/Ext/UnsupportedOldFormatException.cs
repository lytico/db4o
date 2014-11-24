/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// This exception is thrown while reading old database
	/// files for which support has been dropped.
	/// </summary>
	/// <remarks>
	/// This exception is thrown while reading old database
	/// files for which support has been dropped.
	/// </remarks>
	[System.Serializable]
	public class UnsupportedOldFormatException : Db4oFatalException
	{
	}
}
