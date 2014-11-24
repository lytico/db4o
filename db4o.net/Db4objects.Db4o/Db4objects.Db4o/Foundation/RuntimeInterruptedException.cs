/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Foundation
{
	[System.Serializable]
	public class RuntimeInterruptedException : Exception
	{
		public RuntimeInterruptedException(string msg) : base(msg)
		{
		}
	}
}
