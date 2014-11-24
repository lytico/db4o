/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.TA.Nonta
{
	public class StringItem
	{
		public string value;

		public object obj;

		public StringItem()
		{
		}

		public virtual string Value()
		{
			return value;
		}

		public virtual object Object()
		{
			return obj;
		}
	}
}
