/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.ext.*;
import com.db4o.foundation.*;

public class SetSemaphore {

    public void _test() throws InterruptedException {
    	
    	final ExtObjectContainer[] clients = new ExtObjectContainer[5];

        clients[0] = Test.objectContainer();

        Test.ensure(clients[0].setSemaphore("hi", 0));
        Test.ensure(clients[0].setSemaphore("hi", 0));

        if (Test.clientServer) {
        	for (int i = 1; i < clients.length; i++) {
				clients[i] = Test.open();
			}
        	

            Test.ensure(!clients[1].setSemaphore("hi", 0));
            clients[0].releaseSemaphore("hi");
            Test.ensure(clients[1].setSemaphore("hi", 50));
            Test.ensure(!clients[0].setSemaphore("hi", 0));
            Test.ensure(!clients[2].setSemaphore("hi", 0));
            
            
            Thread[] threads = new Thread[clients.length];
            
            for (int i = 0; i < clients.length; i++) {
            	threads[i] = startGetAndReleaseThread(clients[i]);
			}
            
            for (int i = 0; i < threads.length; i++) {
            	threads[i].join();
			}
            
            Runtime4.sleep(50);

            Test.ensure(clients[0].setSemaphore("hi", 0));
            clients[0].close();
            
            threads[2] = startGetAndReleaseThread(clients[2]);
            threads[1] = startGetAndReleaseThread(clients[1]);
            
            threads[1].join();
            threads[2].join();
            
            for (int i = 1; i < 4; i++) {
            	clients[i].close();	
			}
            
            clients[4].setSemaphore("hi", 1000);
        }

    }
    
    private Thread startGetAndReleaseThread(ExtObjectContainer client) {
    	Thread t = new Thread(new GetAndRelease(client), "SetSemaphore.startGetAndReleaseThread");
    	t.start();
    	return t;
    }

    static class GetAndRelease implements Runnable {

        ExtObjectContainer _client;

        public GetAndRelease(ExtObjectContainer client) {
            this._client = client;
        }

        public void run() {
            long time = System.currentTimeMillis();
            Test.ensure(_client.setSemaphore("hi", 50000));
            time = System.currentTimeMillis() - time;
            // System.out.println("Time to get semaphore: " + time);
            Runtime4.sleep(50);

            // System.out.println("About to release semaphore.");
            _client.releaseSemaphore("hi");
        }
    }

}
