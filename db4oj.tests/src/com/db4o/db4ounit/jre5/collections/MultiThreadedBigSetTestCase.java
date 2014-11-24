package com.db4o.db4ounit.jre5.collections;

import java.util.*;
import java.util.concurrent.*;

import com.db4o.collections.*;

import db4ounit.*;
import db4ounit.extensions.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class MultiThreadedBigSetTestCase extends AbstractDb4oTestCase {
	
	static class Item {
	}
	
	public void testMultiThreadedAddRemove() throws InterruptedException {
		final Item[] items = createItems(100);
		
		final Set<Item> set = CollectionFactory.forObjectContainer(db()).newBigSet();
		
		final ExecutorService threadPool = Executors.newFixedThreadPool(20);
		for (int i=0; i<20; ++i) {
			threadPool.execute(new Runnable() { public void run() {
				for (int i=0; i<150; ++i) {
					for (Item item : items) {
						set.add(item);
						Thread.yield();
					}
				}
			}});
			threadPool.execute(new Runnable() { public void run() {
				Item[] transientItems = createItems(20);
				for (Item item : transientItems) {
	                set.add(item);
	                Thread.yield();
                }
				for (Item item : transientItems) {
					Assert.isTrue(set.remove(item));
				}
			}});
		}
		threadPool.shutdown();
		threadPool.awaitTermination(10, TimeUnit.SECONDS);
		
		IteratorAssert.sameContent(Arrays.asList(items), set);
	}

	private Item[] createItems(final int count) {
	    Item[] items = new Item[count];
	    for (int i=0; i<items.length; ++i) {
	    	items[i] = new Item();
	    }
	    return items;
    }
}
