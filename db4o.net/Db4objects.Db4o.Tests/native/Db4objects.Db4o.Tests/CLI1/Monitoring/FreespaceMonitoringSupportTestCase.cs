/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Monitoring;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
    public class FreespaceMonitoringSupportTestCase : PerformanceCounterTestCaseBase
    {
        public class Item{
		
	    }
	

        protected override void  Configure(Db4objects.Db4o.Config.IConfiguration config)
        {
 	         config.Add(new FreespaceMonitoringSupport());
        }

	
	    public void Test(){
            // ensure client is fully connected to the server already
            Db().Commit();
		    AssertMonitoredFreespaceIsCorrect();
		    Item item = new Item();
		    Store(item);
		    Db().Commit();
		    AssertMonitoredFreespaceIsCorrect();
		    Db().Delete(item);
		    Db().Commit();
		    AssertMonitoredFreespaceIsCorrect();
	    }

	    private void AssertMonitoredFreespaceIsCorrect() {
		    IFreespaceManager freespaceManager = FileSession().FreespaceManager();
	        FreespaceCountingVisitor visitor = new FreespaceCountingVisitor();
	        freespaceManager.Traverse(visitor);
	        int freespace = visitor.TotalFreespace;
            int slotCount = visitor.SlotCount;
	        int averageSlotSize = slotCount == 0 ? 0 : freespace/slotCount;
            Assert.AreEqual(freespace, TotalFreespace());
	        Assert.AreEqual(slotCount, SlotCount());
            Assert.AreEqual(averageSlotSize, AverageSlotSize());
        }

        public class FreespaceCountingVisitor : IVisitor4
        {
            private int _totalFreespace;
            private int _slotCount;

            public int TotalFreespace
            {
                get { return _totalFreespace; }
            }

            public int SlotCount
            {
                get { return _slotCount; }
            }

            public void Visit(object obj)
            {
                Slot slot = obj as Slot;
                _totalFreespace += slot.Length();
                _slotCount++;
            }
            
        }
	
	    private int TotalFreespace() {
            return (int)PerformanceCounterSpec.TotalFreespace.PerformanceCounter(FileSession()).RawValue;
	    }
	
	    private int SlotCount() {
	        return (int) PerformanceCounterSpec.FreespaceSlotCount.PerformanceCounter(FileSession()).RawValue;
	    }

        private int AverageSlotSize()
        {
            return (int)PerformanceCounterSpec.FreespaceAverageSlotSize.PerformanceCounter(FileSession()).RawValue;
        }

    }
}
#endif
