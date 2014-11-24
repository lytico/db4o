/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Tests.Data
{
	public sealed class UntypedFieldItem
	{
		private object untyped;

		public UntypedFieldItem()
		{
		}

		public UntypedFieldItem(object value)
		{
			SetUntyped(value);
		}

		public void SetUntyped(object untyped)
		{
			this.untyped = untyped;
		}

		public object GetUntyped()
		{
			return untyped;
		}
	}
}
