/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.TA.Nested;

namespace Db4objects.Db4o.Tests.Common.TA.Nested
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[] { typeof(NestedClassesTestCase) };
		}

		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.TA.Nested.AllTests().RunAll();
		}
	}
}
