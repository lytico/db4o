/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Concurrency;

namespace Db4oUnit.Extensions
{
	public abstract class Db4oConcurrencyTestSuite : AbstractDb4oTestCase, ITestSuiteBuilder
	{
		public virtual IEnumerator GetEnumerator()
		{
			return new Db4oConcurrencyTestSuiteBuilder(Fixture(), TestCases()).GetEnumerator(
				);
		}

		protected abstract override Type[] TestCases();
	}
}
