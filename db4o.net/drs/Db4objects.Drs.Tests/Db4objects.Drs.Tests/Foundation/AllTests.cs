/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Drs.Tests.Foundation;

namespace Db4objects.Drs.Tests.Foundation
{
	public class AllTests : ReflectionTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Drs.Tests.Foundation.AllTests().Run();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(ObjectSetCollection4FacadeTestCase), typeof(Set4Testcase
				), typeof(SignatureTestCase) };
		}
	}
}
