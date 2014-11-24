package com.db4o.drs.test;

import java.util.*;

import com.db4o.*;
import com.db4o.drs.test.data.*;

import db4ounit.*;

public class DateReplicationTestCase extends DrsTestCase {
	
	public void test() {
		final ItemDates item1 = new ItemDates(new Date(0), new Date());
		final ItemDates item2 = new ItemDates(new Date(10000), new Date(System.currentTimeMillis()-10000));
		
		a().provider().storeNew(item1);
		a().provider().storeNew(item2);
		a().provider().commit();
		
		replicateAll(a().provider(), b().provider());
		
		final ObjectSet found = b().provider().getStoredObjects(ItemDates.class);
		Iterator4Assert.sameContent(new Object[] { item2, item1 }, ReplicationTestPlatform.adapt(found.iterator()));
	}

}
