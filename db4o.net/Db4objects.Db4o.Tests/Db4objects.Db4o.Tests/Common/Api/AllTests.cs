/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.Api
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Api.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return ComposeWith();
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(CommonAndLocalConfigurationTestSuite), typeof(Db4oClientServerTestCase
				), typeof(Db4oEmbeddedTestCase), typeof(EnvironmentConfigurationTestCase), typeof(
				StoreAllTestCase) };
		}
		#endif // !SILVERLIGHT
	}
}
