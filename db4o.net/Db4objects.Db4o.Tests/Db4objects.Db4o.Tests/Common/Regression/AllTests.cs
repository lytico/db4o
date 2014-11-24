/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Regression;

namespace Db4objects.Db4o.Tests.Common.Regression
{
	public class AllTests : ComposibleTestSuite
	{
		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(Case1207TestCase), typeof(COR57TestCase), 
				typeof(SetRollbackTestCase) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(COR234TestCase) };
		}
		#endif // !SILVERLIGHT

		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Regression.AllTests().RunSolo();
		}
	}
}
