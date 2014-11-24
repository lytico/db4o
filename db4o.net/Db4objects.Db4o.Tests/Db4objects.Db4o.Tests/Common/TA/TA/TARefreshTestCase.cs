/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.TA;

namespace Db4objects.Db4o.Tests.Common.TA.TA
{
	public class TARefreshTestCase : TransparentActivationTestCaseBase, IOptOutSolo
	{
		public static void Main(string[] args)
		{
			new TARefreshTestCase().RunNetworking();
		}

		private const int ItemDepth = 10;

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			TARefreshTestCase.TAItem item = TARefreshTestCase.TAItem.NewGraph(ItemDepth);
			Store(item);
		}

		public virtual void TestRefresh()
		{
			IExtObjectContainer client1 = OpenNewSession();
			IExtObjectContainer client2 = OpenNewSession();
			TARefreshTestCase.TAItem item1 = QueryRoot(client1);
			TARefreshTestCase.TAItem item2 = QueryRoot(client2);
			TARefreshTestCase.TAItem next1 = item1;
			int value = 10;
			while (next1 != null)
			{
				Assert.AreEqual(value, next1.Value());
				next1 = next1.Next();
				value--;
			}
			TARefreshTestCase.TAItem next2 = item2;
			value = 10;
			while (next2 != null)
			{
				Assert.AreEqual(value, next2.Value());
				next2 = next2.Next();
				value--;
			}
			//update depth = 1
			item1.Value(100);
			item1.Next().Value(200);
			client1.Store(item1, 2);
			client1.Commit();
			AssertItemValue(100, item1);
			AssertItemValue(200, item1.Next());
			AssertItemValue(10, item2);
			AssertItemValue(9, item2.Next());
			//refresh 0
			client2.Refresh(item2, 0);
			AssertItemValue(10, item2);
			AssertItemValue(9, item2.Next());
			//refresh 1
			client2.Refresh(item2, 1);
			AssertItemValue(100, item2);
			AssertItemValue(9, item2.Next());
			//refresh 2
			client2.Refresh(item2, 2);
			AssertItemValue(100, item2);
			AssertItemValue(200, item2.Next());
			next1 = item1;
			value = 1000;
			while (next1 != null)
			{
				next1.Value(value);
				next1 = next1.Next();
				value++;
			}
			client1.Store(item1, 5);
			client1.Commit();
			client2.Refresh(item2, 5);
			next2 = item2;
			for (int i = 1000; i < 1005; i++)
			{
				AssertItemValue(i, next2);
				next2 = next2.Next();
			}
			client1.Close();
			client2.Close();
		}

		private void AssertItemValue(int expectedValue, TARefreshTestCase.TAItem item)
		{
			Assert.AreEqual(expectedValue, item.PassThroughValue());
			Assert.AreEqual(expectedValue, item.Value());
		}

		private TARefreshTestCase.TAItem QueryRoot(IExtObjectContainer client)
		{
			IQuery query = client.Query();
			query.Constrain(typeof(TARefreshTestCase.TAItem));
			query.Descend("_isRoot").Constrain(true);
			return (TARefreshTestCase.TAItem)query.Execute().Next();
		}

		public class TAItem : ActivatableImpl
		{
			public int _value;

			public TARefreshTestCase.TAItem _next;

			public bool _isRoot;

			public static TARefreshTestCase.TAItem NewGraph(int depth)
			{
				TARefreshTestCase.TAItem item = NewTAItem(depth);
				item._isRoot = true;
				return item;
			}

			private static TARefreshTestCase.TAItem NewTAItem(int depth)
			{
				if (depth == 0)
				{
					return null;
				}
				TARefreshTestCase.TAItem root = new TARefreshTestCase.TAItem();
				root._value = depth;
				root._next = NewTAItem(depth - 1);
				return root;
			}

			public virtual int PassThroughValue()
			{
				return _value;
			}

			public virtual int Value()
			{
				Activate(ActivationPurpose.Read);
				return _value;
			}

			public virtual void Value(int value)
			{
				_value = value;
			}

			public virtual TARefreshTestCase.TAItem Next()
			{
				Activate(ActivationPurpose.Read);
				return _next;
			}
		}
	}
}
