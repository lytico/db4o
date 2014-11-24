/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Qlin
{
	/// <summary>
	/// exceptions to signal improper use of the
	/// <see cref="IQLin">IQLin</see>
	/// query interface.
	/// </summary>
	[System.Serializable]
	public class QLinException : Db4oException
	{
		public QLinException(string message) : base(message)
		{
		}

		public QLinException(Exception cause) : base(cause)
		{
		}
	}
}
