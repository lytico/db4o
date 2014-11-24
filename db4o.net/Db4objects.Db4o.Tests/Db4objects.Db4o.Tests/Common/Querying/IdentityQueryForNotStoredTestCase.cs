/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class IdentityQueryForNotStoredTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public string _name;

			public IdentityQueryForNotStoredTestCase.Item _child;

			public Item(IdentityQueryForNotStoredTestCase.Item child, string name)
			{
				_child = child;
				_name = name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			IdentityQueryForNotStoredTestCase.Item item = new IdentityQueryForNotStoredTestCase.Item
				(null, "foo");
			Store(new IdentityQueryForNotStoredTestCase.Item(item, "bar"));
		}

		public virtual void Test()
		{
			IQuery q = NewQuery(typeof(IdentityQueryForNotStoredTestCase.Item));
			q.Descend("_child").Constrain(new IdentityQueryForNotStoredTestCase.Item(null, "foo"
				)).Identity();
			Assert.AreEqual(0, q.Execute().Count);
		}
	}
}
