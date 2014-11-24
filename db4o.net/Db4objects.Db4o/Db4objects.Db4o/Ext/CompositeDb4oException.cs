/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Ext
{
	[System.Serializable]
	public partial class CompositeDb4oException : Exception
	{
		public readonly Exception[] _exceptions;

		public CompositeDb4oException(Exception[] exceptions)
		{
			_exceptions = exceptions;
		}
	}
}
