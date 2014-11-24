package com.db4o.rmi.test;

import java.io.*;
import java.util.*;
import java.util.concurrent.*;

import com.db4o.rmi.*;

public class Feeder extends TheSimplest {

	private Thread clientFeederThread;

	public static class AccConsumer implements ByteArrayConsumer {

		private BlockingQueue<byte[]> q = new LinkedBlockingQueue<byte[]>();

		public void consume(byte[] buffer, int offset, int length) throws IOException {

			q.add(Arrays.copyOfRange(buffer, offset, offset + length));
		}

		public byte[] take() throws InterruptedException {
			return q.poll();
		}

	}

	public void setUp() {

		final AccConsumer serverProducedBuffers = new AccConsumer();

		concreteFacade = new FacadeImpl();

		server = new Distributor<Facade>(serverProducedBuffers, concreteFacade);
		client = new Distributor<Facade>(server, Facade.class);
		
        addCustomClassSerializer();

		client.setFeeder(new Runnable() {

			public void run() {

				try {
					byte[] buffer = serverProducedBuffers.take();

					if (buffer == null) {
						return;
					}
					
					client.consume(buffer, 0, buffer.length);

				} catch (InterruptedException e) {
				} catch (IOException e) {
					e.printStackTrace();
				}

			}
		});

		clientFeederThread = new Thread() {
			@Override
			public void run() {
				try {
					while (true) {
						Thread.sleep(1);
						client.getFeeder().run();
					}
				} catch (InterruptedException e) {
				}
			}
		};
		clientFeederThread.setDaemon(true);
		clientFeederThread.start();

	}

	public void tearDown() throws Exception {
		clientFeederThread.interrupt();
		try {
			clientFeederThread.join();
		} catch (InterruptedException e) {
		}
	}

}
