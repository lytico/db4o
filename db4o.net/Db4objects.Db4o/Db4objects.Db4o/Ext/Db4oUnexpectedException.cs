/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Ext
{
	/// <summary>Unexpected fatal error is encountered.</summary>
	/// <remarks>Unexpected fatal error is encountered.</remarks>
	[System.Serializable]
	public class Db4oUnexpectedException : Exception
	{
		public Db4oUnexpectedException(Exception cause) : base(cause.Message, cause)
		{
		}
	}
}
