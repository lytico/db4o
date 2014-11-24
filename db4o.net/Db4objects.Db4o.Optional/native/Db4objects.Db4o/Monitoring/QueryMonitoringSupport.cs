/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System;
using System.Diagnostics;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Linq;

namespace Db4objects.Db4o.Monitoring
{
	public class NativeQueryMonitoringSupport : IConfigurationItem
	{
		public void Prepare(IConfiguration configuration)
		{
			var common = Db4oLegacyConfigurationBridge.AsCommonConfiguration(configuration);
			common.Environment.Add(new LinqQueryMonitor());
		}

		public void Apply(IInternalObjectContainer container)
		{
			My<LinqQueryMonitor>.Instance.Initialize();

			IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(container);
			
			PerformanceCounter unoptimizedNativeQueriesPerSec = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.UnoptimizedNativeQueriesPerSec, false);
			PerformanceCounter nativeQueriesPerSec = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.NativeQueriesPerSec, false);
			container.GetNativeQueryHandler().QueryExecution += delegate(object sender, QueryExecutionEventArgs args)
			{
				if (args.ExecutionKind == QueryExecutionKind.Unoptimized)
					unoptimizedNativeQueriesPerSec.Increment();

				nativeQueriesPerSec.Increment();
			};

			eventRegistry.Closing += delegate
			{
				nativeQueriesPerSec.RemoveInstance();

				nativeQueriesPerSec.Dispose();
				unoptimizedNativeQueriesPerSec.Dispose();

				container.WithEnvironment(delegate
				{
					My<LinqQueryMonitor>.Instance.Dispose();
				});
			};
		}

		class LinqQueryMonitor : ILinqQueryMonitor
		{
		    private PerformanceCounter _queriesPerSec;
		    private PerformanceCounter _unoptimizedQueriesPerSec;

			public void OnOptimizedQuery()
			{
				QueriesPerSec().Increment();
			}

		    public void OnUnoptimizedQuery()
			{
				QueriesPerSec().Increment();
				UnoptimizedQueriesPerSec().Increment();
			}

		    public void Dispose()
			{
		    	Close(_queriesPerSec);
                Close(_unoptimizedQueriesPerSec);
			}

			private static void Close(IDisposable counter)
			{
				if (null != counter)
				{
					counter.Dispose();
				}
			}

			private PerformanceCounter QueriesPerSec()
            {
                return _queriesPerSec;
            }
            
            private PerformanceCounter UnoptimizedQueriesPerSec()
            {
                return _unoptimizedQueriesPerSec;
            }

			public void Initialize()
			{
				_queriesPerSec = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.LinqQueriesPerSec, false);
				_unoptimizedQueriesPerSec = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.UnoptimizedLinqQueriesPerSec, false);
			}
		}
	}

	public class QueryMonitoringSupport : IConfigurationItem
	{
		public void Prepare(IConfiguration configuration)
		{
		}

		public void Apply(IInternalObjectContainer container)
		{
		    PerformanceCounter queriesPerSec = null;
		    PerformanceCounter classIndexScansPerSec = null;

		    container.WithEnvironment(delegate
            {
		        queriesPerSec = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.QueriesPerSec, false);
                classIndexScansPerSec = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.ClassIndexScansPerSec, false);
            });

			IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(container);
			eventRegistry.QueryFinished += delegate
			{
				queriesPerSec.Increment();
			};
			
			container.Configure().Diagnostic().AddListener(new DiagnosticListener(classIndexScansPerSec));
			
			eventRegistry.Closing += delegate
			{
				queriesPerSec.RemoveInstance();
				classIndexScansPerSec.RemoveInstance();

				queriesPerSec.Dispose();
				classIndexScansPerSec.Dispose();
			};
		}

		class DiagnosticListener : IDiagnosticListener
		{
			private readonly PerformanceCounter _classIndexScansPerSec;

			public DiagnosticListener(PerformanceCounter classIndexScansPerSec)
			{
				_classIndexScansPerSec = classIndexScansPerSec;
			}

			public void OnDiagnostic(IDiagnostic d)
			{
				LoadedFromClassIndex classIndexScan = d as LoadedFromClassIndex;
				if (classIndexScan == null)
					return;

				_classIndexScansPerSec.Increment();
			}
		}
	}
}

#endif