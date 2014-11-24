/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Reflect;

namespace Db4objects.Db4o.Tests.Common.Reflect
{
	public class NoTestConstructorsTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public static int constructorCalls = 0;

			public Item()
			{
				constructorCalls++;
			}
		}

		protected override void Db4oSetupBeforeStore()
		{
			NoTestConstructorsTestCase.Item.constructorCalls = 0;
		}

		protected override void Configure(IConfiguration config)
		{
			config.CallConstructors(true);
			config.TestConstructors(false);
		}

		protected override void Store()
		{
			Store(new NoTestConstructorsTestCase.Item());
			Assert.AreEqual(1, NoTestConstructorsTestCase.Item.constructorCalls);
		}

		public virtual void Test()
		{
			RetrieveOnlyInstance(typeof(NoTestConstructorsTestCase.Item));
			Assert.AreEqual(2, NoTestConstructorsTestCase.Item.constructorCalls);
		}

		public static void Main(string[] args)
		{
			new NoTestConstructorsTestCase().RunAll();
		}
	}
}
