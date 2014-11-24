/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

namespace Db4objects.Db4o.Tests.CLI2.Regression
{
	using System.Collections.Generic;

	using Config;

	using Db4oUnit;
	using Db4oUnit.Extensions;

	public class COR195TestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public int i;

			public Item(int i)
			{
				this.i = i;
			}

			public int I
			{
				get { return i; }
				set { i = value; }
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(Item)).ObjectField("i").Indexed(true);
		}

		protected override void Store()
		{
			for (int i = 0; i < 1000; i++) Store(new Item(i));
		}

		public void TestNativeQueryOnIndex()
		{
			IList<Item> list = Db().Query<Item>(delegate(Item i) { return i.I > 100 && i.I <= 200; });
			Assert.AreEqual(100, list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				Assert.AreEqual(i + 101, list[i].I);
			}
		}
	}
}
