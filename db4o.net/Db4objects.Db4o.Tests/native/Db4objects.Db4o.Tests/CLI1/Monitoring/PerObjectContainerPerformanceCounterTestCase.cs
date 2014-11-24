/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Monitoring;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	public class PerObjectContainerPerformanceCounterTestCase : QueryMonitoringSupportTestCaseBase, IOptOutAllButNetworkingCS, ICustomClientServerConfiguration
	{
		public void ConfigureServer(IConfiguration config)
		{
		}

		public void ConfigureClient(IConfiguration config)
		{
			config.Add(new NativeQueryMonitoringSupport());
		}

		public void Test()
		{
			using (IExtObjectContainer client1 = OpenNewSession())
			using (IExtObjectContainer client2 = OpenNewSession())
			{
				AssertNativeQueriesPerSecond(client1);
				AssertNativeQueriesPerSecond(client2);
			}
		}

		private void AssertNativeQueriesPerSecond(IObjectContainer client)
		{
			AssertCounter(
                PerformanceCounterSpec.NativeQueriesPerSec.PerformanceCounter(client),
				delegate { ExecuteOptimizedNQ(client); });
		}
	}
}
#endif