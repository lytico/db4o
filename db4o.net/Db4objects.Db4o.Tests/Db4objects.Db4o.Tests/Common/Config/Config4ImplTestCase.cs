/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class Config4ImplTestCase : ITestCase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(Config4ImplTestCase)).Run();
		}

		[System.ObsoleteAttribute(@"Test uses deprecated API")]
		public virtual void TestReadAsKeyIsolation()
		{
			Config4Impl config1 = (Config4Impl)Db4oFactory.NewConfiguration();
			Config4Impl config2 = (Config4Impl)Db4oFactory.NewConfiguration();
			Assert.AreNotSame(config1.ReadAs(), config2.ReadAs());
		}
	}
}
