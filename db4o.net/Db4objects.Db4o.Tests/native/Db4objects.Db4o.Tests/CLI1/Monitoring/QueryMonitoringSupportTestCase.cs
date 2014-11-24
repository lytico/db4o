/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT
using System.Diagnostics;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Monitoring;
using Db4objects.Db4o.Monitoring.Internal;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	class QueryMonitoringSupportTestCase : QueryMonitoringSupportTestCaseBase, ICustomClientServerConfiguration
	{
		protected override void Configure(IConfiguration config)
		{
			config.Add(new QueryMonitoringSupport());
		}

		public void ConfigureServer(IConfiguration config)
		{
			Configure(config);
		}

		public void ConfigureClient(IConfiguration config)
		{
		}

        protected override void Db4oSetupAfterStore()
        {
            Container().ProduceClassMetadata(ReflectClass(typeof(Item)));
        }


		public void TestQueriesPerSecond()
		{
            using (PerformanceCounter counter = PerformanceCounterSpec.QueriesPerSec.PerformanceCounter(FileSession()))
            {
                Assert.AreEqual(0, counter.RawValue);

		        ExecuteGetAllQuery();
		        ExecuteClassOnlyQuery();
		        ExecuteOptimizedNQ();
		        ExecuteUnoptimizedNQ();

		        ExecuteOptimizedLinq();
		        ExecuteUnoptimizedLinq();
		        Assert.AreEqual(6, counter.RawValue);
            }
		}

		public void TestClassIndexScansPerSecond()
		{
			AssertCounter(
                PerformanceCounterSpec.ClassIndexScansPerSec.PerformanceCounter(FileSession()),
				ExecuteSodaClassIndexScan);
		}
		
		private void ExecuteClassOnlyQuery()
		{
			NewQuery(typeof(Item)).Execute();
		}

		private void ExecuteGetAllQuery()
		{
			NewQuery().Execute();
		}

		private void ExecuteSodaClassIndexScan()
		{
			IQuery query = NewQuery(typeof(Item));
			query.Descend("_id").Constrain(42);
			query.Execute();
		}
	}
}
#endif
