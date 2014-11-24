/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class IgnoreFieldsTypeHandlerTestCase : AbstractDb4oTestCase
	{
		public class Item1
		{
			public int id1;
		}

		public class Item2 : IgnoreFieldsTypeHandlerTestCase.Item1
		{
			public int id2;
		}

		public class Item3 : IgnoreFieldsTypeHandlerTestCase.Item2
		{
			public int id3;
		}

		public class Item4 : IgnoreFieldsTypeHandlerTestCase.Item3
		{
			public int id4;
		}

		public class Item5 : IgnoreFieldsTypeHandlerTestCase.Item4
		{
			public int id5;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(IgnoreFieldsTypeHandlerTestCase.Item2
				)), IgnoreFieldsTypeHandler.Instance);
			config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(IgnoreFieldsTypeHandlerTestCase.Item4
				)), IgnoreFieldsTypeHandler.Instance);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			IgnoreFieldsTypeHandlerTestCase.Item5 item = new IgnoreFieldsTypeHandlerTestCase.Item5
				();
			item.id1 = 1;
			item.id2 = 2;
			item.id3 = 3;
			item.id4 = 4;
			item.id5 = 5;
			Store(item);
		}

		public virtual void Test()
		{
			IgnoreFieldsTypeHandlerTestCase.Item5 item = (IgnoreFieldsTypeHandlerTestCase.Item5
				)((IgnoreFieldsTypeHandlerTestCase.Item5)RetrieveOnlyInstance(typeof(IgnoreFieldsTypeHandlerTestCase.Item5
				)));
			Assert.AreEqual(1, item.id1);
			Assert.AreEqual(0, item.id2);
			Assert.AreEqual(3, item.id3);
			Assert.AreEqual(0, item.id4);
			Assert.AreEqual(5, item.id5);
		}
	}
}
