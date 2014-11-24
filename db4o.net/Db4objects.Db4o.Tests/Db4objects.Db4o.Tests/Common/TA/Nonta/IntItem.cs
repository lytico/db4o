/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.TA.Nonta
{
	public class IntItem
	{
		public int value;

		public object obj;

		public int i;

		public IntItem()
		{
		}

		public virtual int Value()
		{
			return value;
		}

		public virtual int IntegerValue()
		{
			return i;
		}

		public virtual object Object()
		{
			return obj;
		}
	}
}
