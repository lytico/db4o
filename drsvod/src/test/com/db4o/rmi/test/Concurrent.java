package com.db4o.rmi.test;

import java.io.*;
import java.util.*;
import java.util.concurrent.*;

import com.db4o.rmi.*;

public class Concurrent extends TheSimplest {

	private ConcurrentConsumer serverConsumer;
	private ConcurrentConsumer clientConsumer;
	
	static final byte[] EOF = new byte[0];

	public static class ConcurrentConsumer implements ByteArrayConsumer {

		private ExecutorService executor = Executors.newCachedThreadPool();
        private ByteArrayConsumer consumer;

		public ConcurrentConsumer(ByteArrayConsumer consumer) {
			this.consumer = consumer;
		}

		public void consume(byte[] buffer, int offset, int length) throws IOException {
		    final byte[] buf = Arrays.copyOfRange(buffer, offset, offset+length);
		    executor.execute(new Runnable() {
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
            executor.shutdown();
            try {
                executor.awaitTermination(10, TimeUnit.SECONDS);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }

	}

	public void setUp() {
		super.setUp();
		serverConsumer = new ConcurrentConsumer(client);
		clientConsumer = new ConcurrentConsumer(server);
		server.setConsumer(serverConsumer);
		client.setConsumer(clientConsumer);
	}
	
	public void tearDown() throws Exception {
		serverConsumer.dispose();
		clientConsumer.dispose();
	}
	
}
