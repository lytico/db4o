/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Tests.Data;

namespace Db4oUnit.Tests.Data
{
	public class AllTests : ReflectionTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4oUnit.Tests.Data.AllTests().Run();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(GeneratorsTestCase), typeof(StreamsTestCase) };
		}
	}
}
