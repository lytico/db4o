/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Ordered;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	public class OrderByWithNullValuesTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public int _id;

			public string _name;

			public Item(int id, string name)
			{
				_id = id;
				_name = name;
			}

			public virtual string Name()
			{
				return _name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new OrderByWithNullValuesTestCase.Item(1, "a"));
			Store(new OrderByWithNullValuesTestCase.Item(2, null));
			Store(new OrderByWithNullValuesTestCase.Item(3, "b"));
			Store(new OrderByWithNullValuesTestCase.Item(4, null));
		}

		public virtual void TestOrderByWithNullValues()
		{
			IQuery query = NewQuery();
			query.Constrain(typeof(OrderByWithNullValuesTestCase.Item));
			query.Descend("_name").OrderAscending();
			IObjectSet result = query.Execute();
			Assert.AreEqual(4, result.Count);
			Assert.IsNull(((OrderByWithNullValuesTestCase.Item)result.Next()).Name());
			Assert.IsNull(((OrderByWithNullValuesTestCase.Item)result.Next()).Name());
			Assert.AreEqual("a", ((OrderByWithNullValuesTestCase.Item)result.Next()).Name());
			Assert.AreEqual("b", ((OrderByWithNullValuesTestCase.Item)result.Next()).Name());
		}
	}
}
