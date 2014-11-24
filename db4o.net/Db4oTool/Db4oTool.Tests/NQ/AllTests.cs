using System;
using Db4oUnit;

namespace Db4oTool.Tests.NQ
{
	class AllTests : ReflectionTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[]
			    {
					typeof(DelegateBuildTimeOptimizationTestCase),
					typeof(OptimizedGenericClassTestCase),
					typeof(PredicateBuildTimeOptimizationTestCase),
					typeof(UnoptimizablePredicatesTestCase),
			    };
		}
	}
}
