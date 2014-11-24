/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Monitoring;
using Db4objects.Db4o.Monitoring.Internal;
using System.Diagnostics;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	class NativeQueryMonitoringSupportTestCase : QueryMonitoringSupportTestCaseBase
	{
		protected override void Configure(IConfiguration config)
		{
			config.Add(new NativeQueryMonitoringSupport());
		}
        
        public void TestNativeQueriesPerSecondPerformanceCount()
        {
            using (PerformanceCounter counter = PerformanceCounterSpec.NativeQueriesPerSec.PerformanceCounter(MonitoredContainer()))
			{
				Assert.IsTrue(counter.CounterName.Contains("native queries")); 
			}
        }

		public void TestUnoptimizedNativeQueriesPerSecondPerformanceCount()
        {
            using (PerformanceCounter counter = PerformanceCounterSpec.UnoptimizedNativeQueriesPerSec.PerformanceCounter(MonitoredContainer()))
			{
				Assert.IsTrue(counter.CounterName.Contains("native queries"));
			}
        }

		public void TestNativeQueriesPerSecondWithOptimizedQuery()
		{
			AssertCounter(
                PerformanceCounterSpec.NativeQueriesPerSec.PerformanceCounter(MonitoredContainer()),
				ExecuteOptimizedNQ);
		}

		public void TestNativeQueriesPerSecondWithUnoptimizedQuery()
		{
			AssertCounter(
                PerformanceCounterSpec.NativeQueriesPerSec.PerformanceCounter(MonitoredContainer()),
				ExecuteUnoptimizedNQ);
		}

		public void TestUnoptimizedNativeQueriesPerSecond()
		{
			AssertCounter(
                PerformanceCounterSpec.UnoptimizedNativeQueriesPerSec.PerformanceCounter(MonitoredContainer()),
				ExecuteUnoptimizedNQ);
		}

#if CF_3_5 || NET_3_5
		public void TestLinqQueriesPerSecondWithOptimizedQuery()
		{
			AssertCounter(
                PerformanceCounterSpec.LinqQueriesPerSec.PerformanceCounter(MonitoredContainer()),
				ExecuteOptimizedLinq);
		}

		public void TestLinqQueriesPerSecondWithUnoptimizedQuery()
		{
			AssertCounter(
                PerformanceCounterSpec.LinqQueriesPerSec.PerformanceCounter(MonitoredContainer()),
				ExecuteUnoptimizedLinq);
		}

		public void TestUnoptimizedLinqQueriesPerSecond()
		{
			AssertCounter(
                PerformanceCounterSpec.UnoptimizedLinqQueriesPerSec.PerformanceCounter(MonitoredContainer()),
				ExecuteUnoptimizedLinq);
		}

#endif

	}
}
#endif
