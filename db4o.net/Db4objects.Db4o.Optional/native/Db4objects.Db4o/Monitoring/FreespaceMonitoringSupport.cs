/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System.Diagnostics;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Monitoring
{
    public class FreespaceMonitoringSupport : IConfigurationItem
    {
        public void Prepare(IConfiguration configuration)
        {
        }

        public void Apply(IInternalObjectContainer container)
        {
            if(! (container is LocalObjectContainer) || container.ConfigImpl.IsReadOnly())
			{
			    return;
		    }
		    LocalObjectContainer localObjectContainer = (LocalObjectContainer) container;
		    IFreespaceManager freespaceManager = localObjectContainer.FreespaceManager();
            FreespaceListener freespaceListener = new FreespaceListener(localObjectContainer);
            freespaceManager.Traverse(new FreespaceInitializingVisitor(freespaceListener));

            IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(container);
            eventRegistry.Closing += delegate
            {
                freespaceListener.Dispose();
				freespaceManager.Listener(NullFreespaceListener.Instance);
            };

            freespaceManager.Listener(freespaceListener);
        }

        private class FreespaceInitializingVisitor : IVisitor4
        {
            private readonly IFreespaceListener _freespaceListener;

            public FreespaceInitializingVisitor(IFreespaceListener listener)
            {
                _freespaceListener = listener;
            }

            public void Visit(object obj)
            {
            	Slot slot = (Slot) obj; 
                _freespaceListener.SlotAdded(slot.Length());
            }
        }

        private class FreespaceListener : IFreespaceListener
        {
            public FreespaceListener(IObjectContainer container)
            {
                _totalFreespaceCounter = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.TotalFreespace, container, false);
                _averageSlotSizeCounter = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.FreespaceAverageSlotSize, container, false);
                _freespaceSlotsCounter = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.FreespaceSlotCount, container, false);
                _reusedSlotsCounter = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.FreespaceReusedSlotsPerSec, container, false);
            }

            public void SlotAdded(int size)
            {
                _slotCount++;
                _totalFreespace += size;
                AdjustCounters();
            }

            public void SlotRemoved(int size)
            {
                _reusedSlotsCounter.Increment();
                _slotCount--;
                _totalFreespace -= size;
                AdjustCounters();
            }

            private void AdjustCounters()
            {
                _totalFreespaceCounter.RawValue = _totalFreespace;
            	_averageSlotSizeCounter.RawValue = AverageSlotSize(_slotCount);
                _freespaceSlotsCounter.RawValue = _slotCount;
            }

        	private int AverageSlotSize(int slotCount)
        	{
        		return slotCount == 0 ? 0 : _totalFreespace / slotCount;
        	}

        	public void Dispose()
            {
				_totalFreespaceCounter.RemoveInstance();

                _totalFreespaceCounter.Dispose();
                _averageSlotSizeCounter.Dispose();
                _freespaceSlotsCounter.Dispose();
                _reusedSlotsCounter.Dispose();
            }

			private readonly PerformanceCounter _totalFreespaceCounter;
			private readonly PerformanceCounter _averageSlotSizeCounter;
			private readonly PerformanceCounter _freespaceSlotsCounter;
			private readonly PerformanceCounter _reusedSlotsCounter;

			private int _slotCount;
			private int _totalFreespace;
		}
    }
}
#endif