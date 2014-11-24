/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Soda
{
	public class STValueTypesOrderByTestCase : AbstractDb4oTestCase
	{
		const int ThingsCount = 10;

		private readonly List<object> _items = new List<object>();
		protected override void Store()
		{
			Random rnd = new Random();
			for (int i = 0; i < ThingsCount; i++)
			{
				object item = ItemVariable().New(rnd.Next(1, 1000));
				_items.Add(item);
				Store(item);
			}
		}

		public void TestOrderByDescendingValueType()
		{
			AssertOrderBy(
				CompareDescending,
				delegate(IQuery target) { target.OrderDescending(); });
		}

		public void TestOrderByAscendingValueType()
		{
			AssertOrderBy(
				CompareAscending,
				delegate(IQuery target) { target.OrderAscending(); }
			);
		}

		private void AssertOrderBy(Comparison<object> comparison, Action<IQuery> setOrderBy)
		{
			_items.Sort(comparison);
			IQuery query = NewQuery(ItemVariable().Type());
			setOrderBy(query.Descend("_value"));

			Iterator4Assert.AreEqual(
				_items.ToArray(),
				query.Execute().GetEnumerator());
		}

		private static int CompareDescending(object lhs, object rhs)
		{
			return ItemVariable().Compare(rhs, lhs);
		}

		private static int CompareAscending(object lhs, object rhs)
		{
			return ItemVariable().Compare(lhs, rhs);
		}

		private static IValueTypeFixture ItemVariable()
		{
			return (IValueTypeFixture)STValueTypeOrderByTestSuite.VALUE_TYPE_TYPE_VARIABLE.Value;
		}
	}
}
