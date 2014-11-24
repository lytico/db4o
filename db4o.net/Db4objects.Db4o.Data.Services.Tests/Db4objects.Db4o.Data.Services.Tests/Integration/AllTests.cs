using System;
using Db4oUnit;

namespace Db4objects.Db4o.Data.Services.Tests.Integration
{
	class AllTests : ReflectionTestSuite
	{
		protected override Type[] TestCases()
		{
			return new[]
			       {
			       	typeof(DataServiceHostIntegrationTestCase)
			       };
		}
	}
}
