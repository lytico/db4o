/* Copyright (C) 2010 Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit;

namespace Db4oTool.Tests.TA.Collections
{
	class AllTests : ReflectionTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[]
			       	{
			       		typeof(TADictionaryTestCase),
			       		typeof(TAListTestCase),
			       	};
		}
	}
}
