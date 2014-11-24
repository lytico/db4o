/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
using Db4objects.Db4o.Config;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
	public class ProtectedBaseConstructorTestCase : AbstractDb4oTestCase
	{
		public class Base
		{
			protected Base()
			{
			}
		}

		public class Derived : Base
		{
		}

		protected override void Configure(IConfiguration config)
		{
			config.CallConstructors(true);
		}

		protected override void Store()
		{
			Store(new Derived());
		}

		public void TestClassesWithProtectedBaseConstructorAreReturned()
		{
			Assert.IsNotNull(RetrieveOnlyInstance(typeof(Derived)));
			Assert.IsNotNull(RetrieveOnlyInstance(typeof(Base)));
		}
	}
}
