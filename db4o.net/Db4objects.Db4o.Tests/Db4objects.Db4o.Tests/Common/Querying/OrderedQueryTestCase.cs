/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	/// <exclude></exclude>
	public class OrderedQueryTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new OrderedQueryTestCase().RunSolo();
		}

		public sealed class Item
		{
			public int value;

			public Item(int value)
			{
				this.value = value;
			}
		}

		public class Item2
		{
			public string _name;

			public Item2(string name)
			{
				_name = name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Db().Store(new OrderedQueryTestCase.Item(1));
			Db().Store(new OrderedQueryTestCase.Item(3));
			Db().Store(new OrderedQueryTestCase.Item(2));
		}

		public virtual void TestOrderAscending()
		{
			IQuery query = NewQuery(typeof(OrderedQueryTestCase.Item));
			query.Descend("value").OrderAscending();
			AssertQuery(new int[] { 1, 2, 3 }, query.Execute());
		}

		public virtual void TestOrderDescending()
		{
			IQuery query = NewQuery(typeof(OrderedQueryTestCase.Item));
			query.Descend("value").OrderDescending();
			AssertQuery(new int[] { 3, 2, 1 }, query.Execute());
		}

		public virtual void _testCOR1212()
		{
			Store(new OrderedQueryTestCase.Item2("Item 2"));
			IQuery query = NewQuery();
			query.Constrain(typeof(OrderedQueryTestCase.Item)).Or(query.Constrain(typeof(OrderedQueryTestCase.Item2
				)));
			query.Descend("value").OrderAscending();
			IObjectSet result = query.Execute();
			AssertQuery(new int[] { 1, 2, 3 }, result);
		}

		private void AssertQuery(int[] expected, IObjectSet actual)
		{
			for (int i = 0; i < expected.Length; i++)
			{
				Assert.IsTrue(actual.HasNext());
				Assert.AreEqual(expected[i], ((OrderedQueryTestCase.Item)actual.Next()).value);
			}
			Assert.IsFalse(actual.HasNext());
		}
	}
}
