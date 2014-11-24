/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System.Diagnostics;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;
using Db4objects.Db4o.Monitoring.Internal;

namespace Db4objects.Db4o.Monitoring
{
    public class ReferenceSystemMonitoringSupport : IConfigurationItem
    {
        private class ReferenceSystemListener : IReferenceSystemListener
        {
            private PerformanceCounter _performanceCounter;

            public ReferenceSystemListener(PerformanceCounter counter)
            {
                _performanceCounter = counter;
				_performanceCounter.Disposed += delegate { _performanceCounter = null; };
			}

            public void NotifyReferenceCountChanged(int changedBy)
            {
				if (null != _performanceCounter)
				{
					_performanceCounter.IncrementBy(changedBy);
				}
            }
        }

        private class MonitoringSupportReferenceSystemFactory : IReferenceSystemFactory, IDeepClone
        {
			internal MonitoringSupportReferenceSystemFactory()
			{
			}

			public IReferenceSystem NewReferenceSystem(IInternalObjectContainer container)
            {
            	PerformanceCounter counter = ObjectsInReferenceSystemCounterFor(container);
            	return new MonitoringReferenceSystem(new ReferenceSystemListener(counter));
            }
			
			public object DeepClone(object context)
			{
				return new MonitoringSupportReferenceSystemFactory(this);
			}

        	private PerformanceCounter ObjectsInReferenceSystemCounterFor(IObjectContainer container)
        	{
        		if (_objectsCounter == null)
        		{
        			_objectsCounter = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.ObjectReferenceCount, container, false);
        			IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(container);
        			eventRegistry.Closing += delegate
					{
						_objectsCounter.RemoveInstance();
        				_objectsCounter.Dispose();
        			};
        		}

        		return _objectsCounter;
        	}

        	private MonitoringSupportReferenceSystemFactory(MonitoringSupportReferenceSystemFactory factory)
        	{
        		_objectsCounter = factory._objectsCounter;
        	}

			private PerformanceCounter _objectsCounter;
		}


        public void Prepare(IConfiguration configuration)
        {
            ((Config4Impl)configuration).ReferenceSystemFactory(new MonitoringSupportReferenceSystemFactory());
        }

        public void Apply(IInternalObjectContainer container)
        {

        }
    }
}

#endif
