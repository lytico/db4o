/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Config.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(ClassConfigOverridesGlobalConfigTestSuite
				), typeof(ConfigurationItemTestCase), typeof(ConfigurationOfObjectClassNotSupportedTestCase
				), typeof(Config4ImplTestCase), typeof(CustomStringEncodingTestCase), typeof(EmbeddedConfigurationItemIntegrationTestCase
				), typeof(EmbeddedConfigurationItemUnitTestCase), typeof(GlobalVsNonStaticConfigurationTestCase
				), typeof(LatinStringEncodingTestCase), typeof(ObjectContainerCustomNameTestCase
				), typeof(ObjectTranslatorTestCase), typeof(TransientConfigurationTestSuite), typeof(
				UnicodeStringEncodingTestCase), typeof(UTF8StringEncodingTestCase), typeof(VersionNumbersTestCase
				) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(ConfigurationReuseTestSuite) };
		}
		#endif // !SILVERLIGHT
		// Uses Client/Server
	}
}
