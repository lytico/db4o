/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.TA.TA;

namespace Db4objects.Db4o.Tests.Common.TA.TA
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.TA.TA.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(TAActivateTestCase), typeof(TAArrayTestCase), typeof(TADateArrayTestCase
				), typeof(TADateTestCase), typeof(TAIntTestCase), typeof(TALinkedListTestCase), 
				typeof(TANArrayTestCase), typeof(TARefreshTestCase), typeof(TAStringTestCase) };
		}
	}
}
