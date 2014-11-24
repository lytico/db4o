/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit;

namespace Db4oTool.Tests
{
	class AllTests : ReflectionTestSuite
	{
		protected override Type[] TestCases()
		{
			return new[]
				{
					typeof(ProgramOptionsTestCase),
					typeof(Core.AllTests),
					typeof(NQ.AllTests),
					typeof(TA.AllTests),
				};
		}
	}
}
