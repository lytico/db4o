/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	[System.Serializable]
	public class Db4oRecoverableException : Db4oException
	{
		public Db4oRecoverableException() : base()
		{
		}

		public Db4oRecoverableException(int messageConstant) : base(messageConstant)
		{
		}

		public Db4oRecoverableException(string msg, Exception cause) : base(msg, cause)
		{
		}

		public Db4oRecoverableException(string msg) : base(msg)
		{
		}

		public Db4oRecoverableException(Exception cause) : base(cause)
		{
		}
	}
}
