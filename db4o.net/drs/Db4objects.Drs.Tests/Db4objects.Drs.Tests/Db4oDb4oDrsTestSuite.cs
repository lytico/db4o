/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class Db4oDb4oDrsTestSuite : ITestSuiteBuilder, IDb4oTestCase
	{
		public virtual IEnumerator GetEnumerator()
		{
			return new DrsTestSuiteBuilder(new Db4oDrsFixture("db4o-a"), new Db4oDrsFixture("db4o-b"
				), typeof(Db4oDrsTestSuite)).GetEnumerator();
		}
	}
}
