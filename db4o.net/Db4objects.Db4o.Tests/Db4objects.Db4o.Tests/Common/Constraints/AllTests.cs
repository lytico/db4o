/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Constraints;

namespace Db4objects.Db4o.Tests.Common.Constraints
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Constraints.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(UniqueFieldValueConstraintTestCase), typeof(UniqueFieldIndexWithVersionNumbersTestCase
				), typeof(UniqueFieldValueDoesNotThrowTestCase) };
		}
	}
}
