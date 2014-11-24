/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class SameChildOnDifferentParentQueryTestCase : AbstractDb4oTestCase
	{
		public class Holder
		{
			public SameChildOnDifferentParentQueryTestCase.Item _child;

			public Holder(SameChildOnDifferentParentQueryTestCase.Item belongs)
			{
				_child = belongs;
			}
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			SameChildOnDifferentParentQueryTestCase.Item unique = new SameChildOnDifferentParentQueryTestCase.Item
				("unique");
			SameChildOnDifferentParentQueryTestCase.Item shared = new SameChildOnDifferentParentQueryTestCase.Item
				("shared");
			Store(new SameChildOnDifferentParentQueryTestCase.Holder(shared));
			Store(new SameChildOnDifferentParentQueryTestCase.Holder(unique));
			Store(new SameChildOnDifferentParentQueryTestCase.Holder(shared));
		}

		public virtual void TestUniqueResult()
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(SameChildOnDifferentParentQueryTestCase.Holder));
			query.Descend("_child").Descend("_name").Constrain("unique");
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
			SameChildOnDifferentParentQueryTestCase.Holder holder = ((SameChildOnDifferentParentQueryTestCase.Holder
				)result.Next());
			Assert.AreEqual("unique", holder._child._name);
		}
	}
}
