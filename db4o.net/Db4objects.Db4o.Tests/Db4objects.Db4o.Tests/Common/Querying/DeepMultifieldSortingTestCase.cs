/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class DeepMultifieldSortingTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public int _id;

			public DeepMultifieldSortingTestCase.ItemChild _typedChild;

			public object _untypedChild;

			public Item(int id, DeepMultifieldSortingTestCase.ItemChild typedChild, DeepMultifieldSortingTestCase.ItemChild
				 untypedChild)
			{
				_id = id;
				_typedChild = typedChild;
				_untypedChild = untypedChild;
			}
		}

		public class ItemChild
		{
			public int _id;

			public ItemChild(int id)
			{
				_id = id;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			StoreItems(1, 2, 3);
			StoreItems(3, 2, 3);
			StoreItems(2, 2, 2);
			StoreItems(2, 1, 1);
			StoreItems(2, 3, 3);
		}

		private void StoreItems(int parentId, int typedChildId, int untypedChildId)
		{
			Store(new DeepMultifieldSortingTestCase.Item(parentId, new DeepMultifieldSortingTestCase.ItemChild
				(typedChildId), new DeepMultifieldSortingTestCase.ItemChild(untypedChildId)));
		}

		public virtual void TestTypedChild()
		{
			AssertOrdering("_typedChild");
		}

		/// <summary>#COR-1771 Sorting by untyped fields is not supported.</summary>
		/// <remarks>#COR-1771 Sorting by untyped fields is not supported.</remarks>
		public virtual void _testUntypedChild()
		{
			AssertOrdering("_untypedChild");
		}

		private void AssertOrdering(string childFieldName)
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(DeepMultifieldSortingTestCase.Item));
			query.Descend("_id").OrderAscending();
			query.Descend(childFieldName).Descend("_id").OrderAscending();
			IObjectSet objectSet = query.Execute();
			Assert.AreEqual(5, objectSet.Count);
			DeepMultifieldSortingTestCase.Item lastItem = new DeepMultifieldSortingTestCase.Item
				(0, new DeepMultifieldSortingTestCase.ItemChild(0), null);
			while (objectSet.HasNext())
			{
				DeepMultifieldSortingTestCase.Item item = ((DeepMultifieldSortingTestCase.Item)objectSet
					.Next());
				Assert.IsGreaterOrEqual(lastItem._id, item._id);
				if (item._id == lastItem._id)
				{
					Assert.IsGreaterOrEqual(lastItem._typedChild._id, item._typedChild._id);
				}
				lastItem = item;
			}
		}
	}
}
