/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// The requested operation is not valid in the current state but the database
	/// continues to operate.
	/// </summary>
	/// <remarks>
	/// The requested operation is not valid in the current state but the database
	/// continues to operate.
	/// </remarks>
	[System.Serializable]
	public class Db4oIllegalStateException : Db4oRecoverableException
	{
		public Db4oIllegalStateException(string message) : base(message)
		{
		}

		public Db4oIllegalStateException(Exception cause) : base(cause)
		{
		}
	}
}
