/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

namespace Db4objects.Db4o.NativeQueries
{
	using System;

	public class UnsupportedPredicateException : Exception
	{
		public UnsupportedPredicateException(string reason)
			: base(reason)
		{
		}
	}
}
