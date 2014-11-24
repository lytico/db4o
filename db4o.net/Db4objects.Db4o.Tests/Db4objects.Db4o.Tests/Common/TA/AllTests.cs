/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[] { typeof(Db4objects.Db4o.Tests.Common.TA.Diagnostics.AllTests), 
				typeof(Db4objects.Db4o.Tests.Common.TA.Hierarchy.AllTests), typeof(Db4objects.Db4o.Tests.Common.TA.Nested.AllTests
				), typeof(ReentrantActivationTestCase), typeof(TPExplicitStoreFieldIndexConsistencyTestCase
				) };
		}
	}
}
