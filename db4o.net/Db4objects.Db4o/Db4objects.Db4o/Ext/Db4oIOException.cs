/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when a system IO exception
	/// is encounted by db4o process.
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when a system IO exception
	/// is encounted by db4o process.
	/// </remarks>
	[System.Serializable]
	public class Db4oIOException : Db4oFatalException
	{
		/// <summary>Constructor.</summary>
		/// <remarks>Constructor.</remarks>
		public Db4oIOException() : base()
		{
		}

		public Db4oIOException(string message) : base(message)
		{
		}

		/// <summary>Constructor allowing to specify the causing exception</summary>
		/// <param name="cause">exception cause</param>
		public Db4oIOException(Exception cause) : base(cause.Message, cause)
		{
		}
	}
}
