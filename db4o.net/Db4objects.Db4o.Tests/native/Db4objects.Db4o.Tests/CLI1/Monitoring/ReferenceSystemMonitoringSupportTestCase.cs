/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System;
using Db4objects.Db4o.Monitoring;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
    public class ReferenceSystemMonitoringSupportTestCase : PerformanceCounterTestCaseBase
    {
        public class Item{
		
	    }

        protected override void  Configure(Db4objects.Db4o.Config.IConfiguration config)
        {
 	         config.Add(new ReferenceSystemMonitoringSupport());
        }
	
	    public void TestObjectReferenceCount(){
		    int objectCount = 10;
		    Item[] items = new Item[objectCount];
		    for (int i = 0; i < objectCount; i++) {
			    Assert.AreEqual(ReferenceCountForDb4oDatabase() + i, ObjectReferenceCount());
			    items[i] = new Item();
			    Store(items[i]);
		    }
		    Db().Purge(items[0]);
		    Assert.AreEqual(ReferenceCountForDb4oDatabase() + objectCount -1, ObjectReferenceCount());
	    }
	
	    private int ObjectReferenceCount() {
	        return (int)PerformanceCounterSpec.ObjectReferenceCount.PerformanceCounter(MonitoredContainer()).RawValue;
	    }
	
	    private int ReferenceCountForDb4oDatabase(){
		    if(IsNetworking()){
			    return 0;
		    }
		    return 1;
	    }
    }
}
#endif
