/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Header;

namespace Db4objects.Db4o.Tests.Common.Header
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Header.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(ConfigurationSettingsTestCase), typeof(IdentityTestCase
				), typeof(SimpleTimeStampIdTestCase) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(OldHeaderTest) };
		}
		#endif // !SILVERLIGHT
	}
}
