/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Internal.Convert;

namespace Db4objects.Db4o.Tests.Common.Internal.Convert
{
	public class AllTests : ReflectionTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[] { typeof(ConverterTestCase) };
		}
	}
}
