/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.CS.Config;

namespace Db4objects.Db4o.Tests.Common.CS.Config
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.CS.Config.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(ClientConfigurationItemIntegrationTestCase), typeof(ClientConfigurationItemUnitTestCase
				), typeof(ServerConfigurationItemIntegrationTestCase), typeof(ServerConfigurationItemUnitTestCase
				) };
		}
	}
}
#endif // !SILVERLIGHT
