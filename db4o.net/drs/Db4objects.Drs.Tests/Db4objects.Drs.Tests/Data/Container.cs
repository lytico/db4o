/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class Container
	{
		private Value value;

		public Container()
		{
		}

		public Container(Value value)
		{
			this.SetValue(value);
		}

		public virtual void SetValue(Value value)
		{
			this.value = value;
		}

		public virtual Value GetValue()
		{
			return value;
		}
	}
}
