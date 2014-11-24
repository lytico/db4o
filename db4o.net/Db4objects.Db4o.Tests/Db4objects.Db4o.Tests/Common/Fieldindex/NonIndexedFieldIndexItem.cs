/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	/// <exclude></exclude>
	public class NonIndexedFieldIndexItem : IHasFoo
	{
		public int foo;

		public int indexed;

		public NonIndexedFieldIndexItem()
		{
		}

		public NonIndexedFieldIndexItem(int foo_)
		{
			foo = foo_;
			indexed = foo_;
		}

		public virtual int GetFoo()
		{
			return foo;
		}
	}
}
