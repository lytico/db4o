/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Tests.Data
{
	public class ItemWithUntypedField
	{
		private object _array;

		public ItemWithUntypedField()
		{
		}

		public ItemWithUntypedField(object array)
		{
			Array(array);
		}

		public virtual void Array(object array)
		{
			this._array = array;
		}

		public virtual object Array()
		{
			return _array;
		}
	}
}
