/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test;

import java.util.*;

import com.db4o.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.data.*;
import com.db4o.foundation.*;


public class PerformanceTestCase extends DrsTestCase {
	
	private static int LIST_HOLDER_COUNT = 10;
	
	private static int OBJECT_COUNT = 100;
	
	private static int TOTAL_OBJECT_COUNT = LIST_HOLDER_COUNT + (LIST_HOLDER_COUNT * OBJECT_COUNT * 2); 
	
	
	public void test() {
		System.out.println("**** Simple Replication Performance Test ****");
		long duration = StopWatch.time(new Block4() {
			public void run() {
				System.out.println("Storing in " + a().provider().getName());
				storeInA();
				System.out.println("Replicating " + a().provider().getName() + " to " + b().provider().getName());
				replicate(a().provider(), b().provider());
				System.out.println("Modifying in " + b().provider().getName());
				modifyInB();
				System.out.println("Replicating " + b().provider().getName() + " to " + a().provider().getName());
				replicate(b().provider(), a().provider());
			}
		});
		System.out.println("**** Total time taken " + duration + "ms ****");
	}
	
	private void storeInA() {
		long duration = StopWatch.time(new Block4() {
			public void run() {
				for (int i = 0; i < LIST_HOLDER_COUNT; i++) {
					SimpleListHolder listHolder = new SimpleListHolder("CreatedHolder");
					for (int j = 0; j < OBJECT_COUNT; j++) {
						SimpleItem child = new SimpleItem("CreatedChild");
						SimpleItem parent = new SimpleItem(listHolder, child, "CreatedParent");
						a().provider().storeNew(parent);
						listHolder.add(parent);
					}
					a().provider().storeNew(listHolder);
					a().provider().commit();
				}
				a().provider().commit();
			}
		});
		System.out.println("Time to store " + TOTAL_OBJECT_COUNT + " objects: " + duration + "ms");
	}
	
	private void replicate(final TestableReplicationProviderInside from, final TestableReplicationProviderInside to) {
		long duration = StopWatch.time(new Block4() {
			public void run() {
				replicateAll(from, to);
			}
		});
		System.out.println("Time to replicate " + TOTAL_OBJECT_COUNT + " objects: " + duration + "ms");
	}
	
	private void modifyInB() {
		long duration = StopWatch.time(new Block4() {
			public void run() {
				ObjectSet storedObjects = b().provider().getStoredObjects(SimpleListHolder.class);
				while(storedObjects.hasNext()){
					SimpleListHolder listHolder = (SimpleListHolder) storedObjects.next();
					listHolder.setName("modifiedHolder");
					
					Iterator i = listHolder.getList().iterator();
					while(i.hasNext()){
						SimpleItem parent = (SimpleItem) i.next();
						parent.setValue("ModifiedParent");
						b().provider().update(parent);
						SimpleItem child = parent.getChild();
						child.setValue("ModifiedChild");
						b().provider().update(child);
						b().provider().commit();
					}
					b().provider().update(listHolder);
					b().provider().update(listHolder.getList());
				}
			}
		});
		System.out.println("Time to update " + TOTAL_OBJECT_COUNT + " objects: " + duration + "ms");
	}


}
