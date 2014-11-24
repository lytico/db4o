/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;

namespace Db4oUnit.Extensions.Tests
{
	public class MultipleDb4oTestCase : AbstractDb4oTestCase
	{
		private static int configureCalls = 0;

		public static void ResetConfigureCalls()
		{
			configureCalls = 0;
		}

		public static int ConfigureCalls()
		{
			return configureCalls;
		}

		protected override void Configure(IConfiguration config)
		{
			configureCalls++;
		}

		public virtual void TestFirst()
		{
			Assert.Fail();
		}

		public virtual void TestSecond()
		{
			Assert.Fail();
		}
	}
}
