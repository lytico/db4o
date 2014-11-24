/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

#if !CF && !SILVERLIGHT
using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[]
			       	{
						typeof(ClientConnectionsPerformanceCounterTestCase),
						typeof(ClientNetworkingPerformanceCounterTestCase),
                        typeof(FreespaceMonitoringSupportTestCase),
						typeof(NativeQueryMonitoringSupportTestCase),
                        typeof(ObjectLifecycleMonitoringSupportTestCase),
						typeof(PerformanceCounterLifetimeTestCase),
						typeof(PerObjectContainerPerformanceCounterTestCase),
			       		typeof(QueryMonitoringSupportTestCase),
                        typeof(ReferenceSystemMonitoringSupportTestCase),
						typeof(ServerNetworkingPerformanceCounterTestCase),
					};
		}
	}
}
#endif
