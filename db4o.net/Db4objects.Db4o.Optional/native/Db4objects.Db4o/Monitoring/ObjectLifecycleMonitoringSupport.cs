/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System;
using System.Diagnostics;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Monitoring
{
    public class ObjectLifecycleMonitoringSupport : IConfigurationItem
    {
        public void Prepare(IConfiguration configuration)
        {
        }

        public void Apply(IInternalObjectContainer container)
        {
            PerformanceCounter storedObjectsPerSec =
                Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.ObjectsStoredPerSec, false);
            PerformanceCounter activatedObjectsPerSec =
                Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.ObjectsActivatedPerSec, false);
            PerformanceCounter deactivatedObjectsPerSec =
                Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.ObjectsDeactivatedPerSec, false);
            IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(container);

            EventHandler<ObjectInfoEventArgs> eventHandler = delegate
                                       {
                                           storedObjectsPerSec.Increment();
                                       };
            eventRegistry.Created += eventHandler;
            eventRegistry.Updated += eventHandler;

            eventRegistry.Activated += delegate
                                           {
                                               activatedObjectsPerSec.Increment();
                                           };
            eventRegistry.Deactivated += delegate
                                             {
                                                 deactivatedObjectsPerSec.Increment();
                                             };

            eventRegistry.Closing += delegate
                                        {
                                            storedObjectsPerSec.Dispose();
                                            activatedObjectsPerSec.Dispose();
                                            deactivatedObjectsPerSec.Dispose();

                                            storedObjectsPerSec.RemoveInstance();
                                        };
            if (container.IsClient)
            {
                return;
            }

            PerformanceCounter deletedObjectsPerSec =
                Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.ObjectsDeletedPerSec, false);
            eventRegistry.Deleted += delegate
                                         {
                                             deletedObjectsPerSec.Increment();
                                         };

            eventRegistry.Closing += delegate
            {
                deletedObjectsPerSec.Dispose();
            };



        }
    }
}

#endif