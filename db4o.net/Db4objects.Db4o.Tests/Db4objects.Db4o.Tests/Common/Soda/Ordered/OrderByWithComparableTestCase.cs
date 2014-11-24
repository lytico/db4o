/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.Soda.Ordered;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	public class OrderByWithComparableTestCase : AbstractDb4oTestCase
	{
		public class ItemComparable : IComparable, IActivatable
		{
			[System.NonSerialized]
			private IActivator _activator;

			public int _id;

			public ItemComparable(int id)
			{
				_id = id;
			}

			public virtual int CompareTo(object other)
			{
				Activate(ActivationPurpose.Read);
				OrderByWithComparableTestCase.ItemComparable cmp = (OrderByWithComparableTestCase.ItemComparable
					)other;
				if (_id == cmp.Id())
				{
					return 0;
				}
				return _id < cmp.Id() ? -1 : 1;
			}

			public virtual int Id()
			{
				Activate(ActivationPurpose.Read);
				return _id;
			}

			public override string ToString()
			{
				Activate(ActivationPurpose.Read);
				return "ItemComparable(" + _id + ")";
			}

			public virtual void Bind(IActivator activator)
			{
				if (_activator == activator)
				{
					return;
				}
				if (_activator != null && activator != null)
				{
					throw new Exception();
				}
				_activator = activator;
			}

			public virtual void Activate(ActivationPurpose purpose)
			{
				if (_activator != null)
				{
					_activator.Activate(purpose);
				}
			}
		}

		public class Item
		{
			public int _id;

			public OrderByWithComparableTestCase.ItemComparable _itemCmp;

			public Item(int id, OrderByWithComparableTestCase.ItemComparable itemCmp)
			{
				_id = id;
				_itemCmp = itemCmp;
			}

			public virtual OrderByWithComparableTestCase.ItemComparable ItemCmp()
			{
				return _itemCmp;
			}

			public override string ToString()
			{
				return "Item(" + _itemCmp + ")";
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentActivationSupport());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new OrderByWithComparableTestCase.Item(3, new OrderByWithComparableTestCase.ItemComparable
				(2)));
			Store(new OrderByWithComparableTestCase.Item(2, null));
			Store(new OrderByWithComparableTestCase.Item(1, new OrderByWithComparableTestCase.ItemComparable
				(1)));
			Store(new OrderByWithComparableTestCase.Item(4, null));
		}

		public virtual void TestOrderByWithEnums()
		{
			IQuery query = NewQuery();
			query.Constrain(typeof(OrderByWithComparableTestCase.Item));
			query.Descend("_id").Constrain(1).Or(query.Descend("_id").Constrain(3));
			query.Descend("_itemCmp").OrderAscending();
			IObjectSet result = query.Execute();
			Assert.AreEqual(2, result.Count);
			Assert.AreEqual(1, ((OrderByWithComparableTestCase.Item)result.Next()).ItemCmp().
				Id());
			Assert.AreEqual(2, ((OrderByWithComparableTestCase.Item)result.Next()).ItemCmp().
				Id());
		}

		public virtual void TestOrderByWithNullValues()
		{
			IQuery query = NewQuery();
			query.Constrain(typeof(OrderByWithComparableTestCase.Item));
			query.Descend("_itemCmp").OrderAscending();
			IObjectSet result = query.Execute();
			Assert.AreEqual(4, result.Count);
			Assert.IsNull(((OrderByWithComparableTestCase.Item)result.Next()).ItemCmp());
			Assert.IsNull(((OrderByWithComparableTestCase.Item)result.Next()).ItemCmp());
			Assert.AreEqual(1, ((OrderByWithComparableTestCase.Item)result.Next()).ItemCmp().
				Id());
			Assert.AreEqual(2, ((OrderByWithComparableTestCase.Item)result.Next()).ItemCmp().
				Id());
		}
	}
}
