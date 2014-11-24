/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.TA.Mixed;

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.TA.Mixed.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(LinkedArrayTestCase), typeof(MixedActivateTestCase), typeof(
				MixedArrayTestCase), typeof(MixedTARefreshTestCase), typeof(NNTTestCase), typeof(
				NTNTestCase), typeof(NTTestCase), typeof(TNTTestCase) };
		}
	}
}
