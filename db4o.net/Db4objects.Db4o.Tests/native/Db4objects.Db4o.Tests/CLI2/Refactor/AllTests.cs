/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Refactor
{
	public class AllTests : Db4oTestSuite
	{
		#region Overrides of Db4oTestSuite

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(AddIndexedValueTypeFieldTestCase) };
		}

		#endregion
	}
}
