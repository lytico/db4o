/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.cs;

import com.db4o.cs.internal.*;
import com.db4o.cs.internal.ClientObjectContainer.MessageListener;
import com.db4o.cs.internal.messages.*;
import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class IsAliveConcurrencyTestCase extends Db4oClientServerTestCase implements OptOutAllButNetworkingCS {

	private volatile boolean processingMessage = false;
	
	public void testIsAliveInMultiThread() throws InterruptedException {
			
		final BlockingQueue4<Object> barrier =new BlockingQueue<Object>();
		
		client = (ClientObjectContainer) openNewSession();
		
		client.messageListener(new MessageListener() {			
			public void onMessage(Msg msg) {
				
				if (msg instanceof MQueryExecute) {
					processingMessage = true;
					barrier.add(new Object());
					Runtime4.sleep(500);
					processingMessage = false;					
				}
				else if (msg instanceof MIsAlive) {
					Assert.isFalse(processingMessage);					
				}
			}
		});
		
		Thread workThread = new Thread(new Runnable() {
			public void run() {
				client.queryByExample(Item.class);
			}}, "Quering");
		
		workThread.setDaemon(true);
		workThread.start();
		
		barrier.next();
		client.isAlive();		
	}

	protected void store() {
		for (int i = 0; i < 10; ++i) {
			store(new Item());
		}
	}

	public static class Item {
	}
	
	private static ClientObjectContainer client;
}
