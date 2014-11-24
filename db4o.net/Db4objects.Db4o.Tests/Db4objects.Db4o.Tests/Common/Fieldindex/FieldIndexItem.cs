/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class FieldIndexItem : IHasFoo
	{
		public int foo;

		public FieldIndexItem()
		{
		}

		public FieldIndexItem(int foo_)
		{
			foo = foo_;
		}

		public virtual int GetFoo()
		{
			return foo;
		}
	}
}
