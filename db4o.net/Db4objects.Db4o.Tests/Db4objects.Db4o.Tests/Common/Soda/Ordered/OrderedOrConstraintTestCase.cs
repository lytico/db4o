/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Ordered;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	/// <summary>COR-1062</summary>
	public class OrderedOrConstraintTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public Item(int int_, bool boolean_)
			{
				_int = int_;
				_boolean = boolean_;
			}

			public int _int;

			public bool _boolean;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new OrderedOrConstraintTestCase.Item(10, false));
			Store(new OrderedOrConstraintTestCase.Item(4, true));
			base.Store();
		}

		public virtual void Test()
		{
			IQuery query = NewQuery(typeof(OrderedOrConstraintTestCase.Item));
			IConstraint c1 = query.Descend("_int").Constrain(9).Greater();
			IConstraint c2 = query.Descend("_boolean").Constrain(true);
			c1.Or(c2);
			query.Descend("_int").OrderAscending();
			IObjectSet objectSet = query.Execute();
			Assert.AreEqual(2, objectSet.Count);
			OrderedOrConstraintTestCase.Item item = ((OrderedOrConstraintTestCase.Item)objectSet
				.Next());
			Assert.AreEqual(4, item._int);
			item = ((OrderedOrConstraintTestCase.Item)objectSet.Next());
			Assert.AreEqual(10, item._int);
		}
	}
}
