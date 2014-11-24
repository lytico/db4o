/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Tests.Common.TA.Nonta
{
	public class DateItem
	{
		public DateTime _typed;

		public object _untyped;

		public virtual DateTime GetTyped()
		{
			return _typed;
		}

		public virtual object GetUntyped()
		{
			return _untyped;
		}

		public override string ToString()
		{
			return _typed.ToString();
		}
	}
}
