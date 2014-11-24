/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Mixed;

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
	public class MixedTARefreshTestCase : TransparentActivationTestCaseBase, IOptOutSolo
	{
		public static void Main(string[] args)
		{
			new MixedTARefreshTestCase().RunNetworking();
		}

		private const int ItemDepth = 10;

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			MixedTARefreshTestCase.Item item = MixedTARefreshTestCase.TAItem.NewItem(ItemDepth
				);
			item._isRoot = true;
			Store(item);
		}

		public virtual void TestRefresh()
		{
			IExtObjectContainer client1 = OpenNewSession();
			IExtObjectContainer client2 = OpenNewSession();
			MixedTARefreshTestCase.Item item1 = RetrieveInstance(client1);
			MixedTARefreshTestCase.Item item2 = RetrieveInstance(client2);
			MixedTARefreshTestCase.Item next1 = item1;
			int value = 10;
			while (next1 != null)
			{
				Assert.AreEqual(value, next1.GetValue());
				next1 = next1.Next();
				value--;
			}
			MixedTARefreshTestCase.Item next2 = item2;
			value = 10;
			while (next2 != null)
			{
				Assert.AreEqual(value, next2.GetValue());
				next2 = next2.Next();
				value--;
			}
			item1.SetValue(100);
			item1.Next().SetValue(200);
			client1.Store(item1, 2);
			client1.Commit();
			Assert.AreEqual(100, item1.GetValue());
			Assert.AreEqual(200, item1.Next().GetValue());
			Assert.AreEqual(10, item2.GetValue());
			Assert.AreEqual(9, item2.Next().GetValue());
			//refresh 0
			client2.Refresh(item2, 0);
			Assert.AreEqual(10, item2.GetValue());
			Assert.AreEqual(9, item2.Next().GetValue());
			//refresh 1
			client2.Refresh(item2, 1);
			Assert.AreEqual(100, item2.GetValue());
			Assert.AreEqual(9, item2.Next().GetValue());
			//refresh 2
			client2.Refresh(item2, 2);
			Assert.AreEqual(100, item2.GetValue());
			//FIXME: maybe a bug
			//Assert.areEqual(200, item2.next().getValue());
			next1 = item1;
			value = 1000;
			while (next1 != null)
			{
				next1.SetValue(value);
				next1 = next1.Next();
				value++;
			}
			client1.Store(item1, 5);
			client1.Commit();
			client2.Refresh(item2, 5);
			next2 = item2;
			for (int i = 1000; i < 1005; i++)
			{
				Assert.AreEqual(i, next2.GetValue());
				next2 = next2.Next();
			}
			client1.Close();
			client2.Close();
		}

		private MixedTARefreshTestCase.Item RetrieveInstance(IExtObjectContainer client)
		{
			IQuery query = client.Query();
			query.Constrain(typeof(MixedTARefreshTestCase.Item));
			query.Descend("_isRoot").Constrain(true);
			return (MixedTARefreshTestCase.Item)query.Execute().Next();
		}

		public class Item
		{
			public int _value;

			public MixedTARefreshTestCase.Item _next;

			public bool _isRoot;

			public Item()
			{
			}

			public Item(int value)
			{
				//
				_value = value;
			}

			public static MixedTARefreshTestCase.Item NewItem(int depth)
			{
				if (depth == 0)
				{
					return null;
				}
				MixedTARefreshTestCase.Item header = new MixedTARefreshTestCase.Item(depth);
				header._next = MixedTARefreshTestCase.TAItem.NewTAITem(depth - 1);
				return header;
			}

			public virtual int GetValue()
			{
				return _value;
			}

			public virtual void SetValue(int value)
			{
				_value = value;
			}

			public virtual MixedTARefreshTestCase.Item Next()
			{
				return _next;
			}
		}

		public class TAItem : MixedTARefreshTestCase.Item, IActivatable
		{
			[System.NonSerialized]
			private IActivator _activator;

			public TAItem(int value) : base(value)
			{
			}

			public static MixedTARefreshTestCase.TAItem NewTAITem(int depth)
			{
				if (depth == 0)
				{
					return null;
				}
				MixedTARefreshTestCase.TAItem header = new MixedTARefreshTestCase.TAItem(depth);
				header._next = MixedTARefreshTestCase.Item.NewItem(depth - 1);
				return header;
			}

			public override int GetValue()
			{
				Activate(ActivationPurpose.Read);
				return _value;
			}

			public override MixedTARefreshTestCase.Item Next()
			{
				Activate(ActivationPurpose.Read);
				return _next;
			}

			public virtual void Activate(ActivationPurpose purpose)
			{
				if (_activator == null)
				{
					return;
				}
				_activator.Activate(purpose);
			}

			public virtual void Bind(IActivator activator)
			{
				_activator = activator;
			}
		}
	}
}
