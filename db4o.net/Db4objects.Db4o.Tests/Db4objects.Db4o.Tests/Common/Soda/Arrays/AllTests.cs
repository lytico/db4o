/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Soda.Arrays;
using Db4objects.Db4o.Tests.Common.Soda.Arrays.Object;
using Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed;
using Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped;

namespace Db4objects.Db4o.Tests.Common.Soda.Arrays
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[] { typeof(STArrMixedTestCase), typeof(STArrStringOTestCase), typeof(
				STArrStringONTestCase), typeof(STArrStringTTestCase), typeof(STArrStringTNTestCase
				), typeof(STArrStringUTestCase), typeof(STArrStringUNTestCase), typeof(STArrIntegerOTestCase
				), typeof(STArrIntegerONTestCase), typeof(STArrIntegerTTestCase), typeof(STArrIntegerTNTestCase
				), typeof(STArrIntegerUTestCase), typeof(STArrIntegerUNTestCase), typeof(STArrIntegerWTTestCase
				), typeof(STArrIntegerWTONTestCase), typeof(STArrIntegerWUONTestCase), typeof(ArrayDescendSubQueryTestCase
				) };
		}
	}
}
