package com.db4o.rmi.test;

import java.io.*;
import java.util.*;
import java.util.concurrent.*;

import com.db4o.rmi.*;

public class Decoupled extends TheSimplest {

	private DecoupledConsumer serverConsumer;
	private DecoupledConsumer clientConsumer;
	
	public static class DecoupledConsumer implements ByteArrayConsumer {

		private ThreadLocal<ExecutorService> executor = new ThreadLocal<ExecutorService>() {
		    @Override
		    protected ExecutorService initialValue() {
		        return Executors.newFixedThreadPool(1);
		    }
		};
        private final ByteArrayConsumer consumer;

		public DecoupledConsumer(final ByteArrayConsumer consumer) {
			this.consumer = consumer;
		}

		public void consume(byte[] buffer, int offset, int length) throws IOException {
		    final byte[] buf = Arrays.copyOfRange(buffer, offset, offset+length);
		    executor.get().execute(new Runnable() {
                public void run() {
                    try {
                        consumer.consume(buf, 0, buf.length);
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                }
            });
		}

		public void dispose() {
		    executor.get().shutdown();
		    try {
		        executor.get().awaitTermination(10, TimeUnit.SECONDS);
			} catch (InterruptedException e) {
			    e.printStackTrace();
			}
		}

	}

	public void setUp() {
		super.setUp();
		serverConsumer = new DecoupledConsumer(client);
		clientConsumer = new DecoupledConsumer(server);
		server.setConsumer(serverConsumer);
		client.setConsumer(clientConsumer);
	}
	
	public void tearDown() throws Exception {
		serverConsumer.dispose();
		clientConsumer.dispose();
	}

}
