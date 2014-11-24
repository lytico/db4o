/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
	public class UntypedDelegateArrayTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public Action<string> typed;
			public Action<string> untyped;
			public Action<string>[] typedArray;
			public object[] untypedArray;

			public Item(Action<string> action)
			{
				typed = action;
				untyped = action;
				typedArray = new Action<string>[1] {action};
				untypedArray = new object[] {action};
			}
		}

		protected override void Configure(Config.IConfiguration config)
		{
			config.ExceptionsOnNotStorable(true);
			config.CallConstructors(true);
		}

		protected override void Store()
		{
			Store(new Item(StringAction));
		}

		public void Test()
		{
			Item item = RetrieveOnlyInstance<Item>();
			Assert.IsNull(item.typed);
			Assert.IsNull(item.untyped);
			ArrayAssert.AreEqual(new object[1], item.untypedArray);
			ArrayAssert.AreEqual(new Action<string>[1], item.typedArray);
		}

		private static void StringAction(string s)
		{
			throw new NotImplementedException();
		}
	}
}
