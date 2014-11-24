/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Internal.Query;

namespace Db4objects.Db4o.Tests.Common.Internal.Query
{
	public class SodaQueryComparatorTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			StoreItem(1, "bb", "ca");
			StoreItem(2, "aa", "cb");
		}

		public virtual void TestNullInThePath()
		{
			Store(new SodaQueryComparatorTestCase.Item(3, "cc", null));
			int[] expectedItemIds = new int[] { 3, 1, 2 };
			AssertQuery(expectedItemIds, new SodaQueryComparator.Ordering[] { Ascending(new string
				[] { "child", "name" }) });
		}

		public virtual void TestFirstLevelAscending()
		{
			int[] expectedItems = new int[] { 2, 1 };
			AssertQuery(expectedItems, new SodaQueryComparator.Ordering[] { Ascending(new string
				[] { "name" }) });
		}

		public virtual void TestSecondLevelAscending()
		{
			int[] expectedItems = new int[] { 1, 2 };
			AssertQuery(expectedItems, new SodaQueryComparator.Ordering[] { Ascending(new string
				[] { "child", "name" }) });
		}

		public virtual void TestFirstLevelThenSecondLevel()
		{
			StoreItem(3, "aa", "cc");
			StoreItem(4, "bb", "cc");
			int[] expectedItems = new int[] { 2, 3, 1, 4 };
			AssertQuery(expectedItems, new SodaQueryComparator.Ordering[] { Ascending(new string
				[] { "name" }), Ascending(new string[] { "child", "name" }) });
		}

		public virtual void TestSecondLevelThenFirstLevel()
		{
			StoreItem(3, "cc", "ca");
			StoreItem(4, "cc", "ce");
			int[] expectedItems = new int[] { 1, 3, 2, 4 };
			AssertQuery(expectedItems, new SodaQueryComparator.Ordering[] { Ascending(new string
				[] { "child", "name" }), Ascending(new string[] { "name" }) });
		}

		public virtual void TestFirstLevelDescending()
		{
			int[] expectedItems = new int[] { 1, 2 };
			AssertQuery(expectedItems, new SodaQueryComparator.Ordering[] { Descending(new string
				[] { "name" }) });
		}

		public virtual void TestSecondLevelDescending()
		{
			int[] expectedItems = new int[] { 2, 1 };
			AssertQuery(expectedItems, new SodaQueryComparator.Ordering[] { Descending(new string
				[] { "child", "name" }) });
		}

		public virtual void TestFirstLevelThenSecondLevelDescending()
		{
			StoreItem(3, "aa", "cc");
			StoreItem(4, "bb", "cc");
			int[] expectedItems = new int[] { 4, 1, 3, 2 };
			AssertQuery(expectedItems, new SodaQueryComparator.Ordering[] { Descending(new string
				[] { "name" }), Descending(new string[] { "child", "name" }) });
		}

		public virtual void TestSecondLevelThenFirstLevelDescending()
		{
			StoreItem(3, "cc", "ca");
			StoreItem(4, "cc", "ce");
			int[] expectedItems = new int[] { 4, 2, 3, 1 };
			AssertQuery(expectedItems, new SodaQueryComparator.Ordering[] { Descending(new string
				[] { "child", "name" }), Descending(new string[] { "name" }) });
		}

		public virtual void TestFirstLevelAscendingThenSecondLevelDescending()
		{
			StoreItem(3, "aa", "cc");
			StoreItem(4, "bb", "cc");
			int[] expectedItems = new int[] { 3, 2, 4, 1 };
			AssertQuery(expectedItems, new SodaQueryComparator.Ordering[] { Ascending(new string
				[] { "name" }), Descending(new string[] { "child", "name" }) });
		}

		public virtual void TestSecondLevelAscendingThenFirstLevelDescending()
		{
			StoreItem(3, "cc", "ca");
			StoreItem(4, "cc", "ce");
			int[] expectedItems = new int[] { 3, 1, 2, 4 };
			AssertQuery(expectedItems, new SodaQueryComparator.Ordering[] { Ascending(new string
				[] { "child", "name" }), Descending(new string[] { "name" }) });
		}

		private SodaQueryComparator.Ordering Ascending(string[] fieldPath)
		{
			return new SodaQueryComparator.Ordering(SodaQueryComparator.Direction.Ascending, 
				fieldPath);
		}

		private SodaQueryComparator.Ordering Descending(string[] fieldPath)
		{
			return new SodaQueryComparator.Ordering(SodaQueryComparator.Direction.Descending, 
				fieldPath);
		}

		private void StoreItem(int id, string name, string childName)
		{
			Store(new SodaQueryComparatorTestCase.Item(id, name, new SodaQueryComparatorTestCase.ItemChild
				(childName)));
		}

		private void AssertQuery(int[] expectedItemIds, SodaQueryComparator.Ordering[] orderings
			)
		{
			long[] ids = NewQuery(typeof(SodaQueryComparatorTestCase.Item)).Execute().Ext().GetIDs
				();
			IList sorted = new SodaQueryComparator(FileSession(), typeof(SodaQueryComparatorTestCase.Item
				), orderings).Sort(ids);
			Iterator4Assert.AreEqual(Iterators.Map(expectedItemIds, oidByItemId), Iterators.Iterator
				(sorted));
		}

		private sealed class _IFunction4_121 : IFunction4
		{
			public _IFunction4_121(SodaQueryComparatorTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object id)
			{
				int oid = this._enclosing.ItemByName(((int)id));
				//			System.out.println(id + " -> " + oid);
				return oid;
			}

			private readonly SodaQueryComparatorTestCase _enclosing;
		}

		internal readonly IFunction4 oidByItemId;

		private int ItemByName(int id)
		{
			IQuery query = NewQuery(typeof(SodaQueryComparatorTestCase.Item));
			query.Descend("id").Constrain(id);
			return (int)query.Execute().Ext().GetIDs()[0];
		}

		public class Item
		{
			public Item(int id, string name, SodaQueryComparatorTestCase.ItemChild child)
			{
				this.id = id;
				this.name = name;
				this.child = child;
			}

			public int id;

			public string name;

			public SodaQueryComparatorTestCase.ItemChild child;
		}

		public class ItemChild
		{
			public ItemChild(string name)
			{
				this.name = name;
			}

			public string name;
		}

		public SodaQueryComparatorTestCase()
		{
			oidByItemId = new _IFunction4_121(this);
		}
	}
}
