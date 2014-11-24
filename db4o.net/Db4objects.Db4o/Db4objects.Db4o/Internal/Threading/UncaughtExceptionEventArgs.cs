/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Internal.Threading
{
	public class UncaughtExceptionEventArgs : EventArgs
	{
		private System.Exception _exception;

		public UncaughtExceptionEventArgs(System.Exception e)
		{
			_exception = e;
		}

		public virtual System.Exception Exception
		{
			get
			{
				return _exception;
			}
		}
	}
}
