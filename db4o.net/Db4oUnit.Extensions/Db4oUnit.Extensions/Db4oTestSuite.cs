/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4oUnit.Extensions
{
	/// <summary>Base class for composable db4o test suites (AllTests classes inside each package, for instance).
	/// 	</summary>
	/// <remarks>Base class for composable db4o test suites (AllTests classes inside each package, for instance).
	/// 	</remarks>
	public abstract class Db4oTestSuite : AbstractDb4oTestCase, ITestSuiteBuilder
	{
		public virtual IEnumerator GetEnumerator()
		{
			return new Db4oTestSuiteBuilder(Fixture(), TestCases()).GetEnumerator();
		}

		protected abstract override Type[] TestCases();
	}
}
