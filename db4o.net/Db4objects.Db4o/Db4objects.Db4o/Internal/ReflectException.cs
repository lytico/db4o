/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Internal
{
	/// <summary>
	/// db4o-specific exception.<br />
	/// <br />
	/// This exception is thrown when one of the db4o reflection methods fails.
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br />
	/// <br />
	/// This exception is thrown when one of the db4o reflection methods fails.
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Reflect">Db4objects.Db4o.Reflect</seealso>
	[System.Serializable]
	public class ReflectException : Db4oRecoverableException
	{
		public ReflectException(string msg, Exception cause) : base(msg, cause)
		{
		}

		/// <summary>Constructor with the cause exception</summary>
		/// <param name="cause">cause exception</param>
		public ReflectException(Exception cause) : base(cause)
		{
		}

		/// <summary>Constructor with message</summary>
		/// <param name="message">detailed explanation</param>
		public ReflectException(string message) : base(message)
		{
		}
	}
}
