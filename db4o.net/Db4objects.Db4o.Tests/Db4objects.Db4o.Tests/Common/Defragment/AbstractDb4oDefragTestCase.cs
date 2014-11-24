/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.Common.Defragment
{
	public abstract class AbstractDb4oDefragTestCase : ITestSuiteBuilder
	{
		public virtual string GetLabel()
		{
			return "DefragAllTestCase: " + TestSuite().FullName;
		}

		public abstract Type TestSuite();

		public virtual IEnumerator GetEnumerator()
		{
			return new Db4oTestSuiteBuilder(new Db4oDefragSolo(), TestSuite()).GetEnumerator(
				);
		}
	}
}
