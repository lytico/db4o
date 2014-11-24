package com.db4o.drs.versant.ipc.tcp;

import java.io.*;
import java.net.*;

import com.db4o.drs.versant.ipc.*;
import com.db4o.rmi.*;

public class TcpClient implements ClientChannelControl {

	private static final long CONNECTION_TIMEOUT_IN_MILLIS = 5 * 1000;

	volatile boolean running = true;
	
	private Thread thread;

	private Socket socket;

	private Distributor<EventProcessor> remotePeer;

	private int port;

	private String host;
	

	public TcpClient(String host, int port) {
		this.host = host;
		this.port = port;
	}
	
	public EventProcessor sync() {
		ensureInited();
		return remotePeer.sync();
	}

	public EventProcessor async() {
		ensureInited();
		return remotePeer.async();
	}
	
	private void ensureInited() {
		if (remotePeer == null) {
			init();
		}
	}

	public void stop() {
		running = false;
		if (socket == null) {
			return;
		}
		try {
			socket.close();
		} catch (IOException e) {
		}
	}
	
	public void join() throws InterruptedException {
		if (thread == null) {
			return;
		}
		thread.join();
	}
	
	private synchronized void init() {
		
		if (remotePeer != null) {
			return;
		}

		try {
			socket = connect();

			if (socket == null) {
			}

			final DataOutputStream out = new DataOutputStream(new BufferedOutputStream(socket.getOutputStream()));
			final DataInputStream in = new DataInputStream(new BufferedInputStream(socket.getInputStream()));

			remotePeer = new Distributor<EventProcessor>(new ByteArrayConsumer() {

				public void consume(byte[] buffer, int offset, int length) throws IOException {
					out.writeInt(length);
					out.write(buffer, offset, length);
					out.flush();
				}
			}, EventProcessor.class);
			
			thread = new Thread("TcpClient receiver thread") {
				@Override
				public void run() {
					try {
						while(true) {
							TcpCommunicationNetwork.feed(in, remotePeer);
						}
					} catch (IOException e) {
//						e.printStackTrace();
					}
				}
			};
			thread.setDaemon(true);
			thread.start();
			
		} catch (IOException e) {
			throw new RuntimeException(e);
		}

	}
	
	private Socket connect() throws IOException {
		Socket s = null;
		long startedTime = System.currentTimeMillis();
		int count = 0;
		while (running) {
			try {
				s = new Socket(host, port);
				break;
			} catch (ConnectException e) {
				count++ ;
				long elapsed = System.currentTimeMillis() - startedTime;
				if (elapsed > CONNECTION_TIMEOUT_IN_MILLIS) {
					throw new ConnectionTimeoutException(host, port, elapsed, count, e);
				}
				try {
					Thread.sleep(100);
				} catch (InterruptedException e1) {
					throw new RuntimeException(e1);
				}
			} catch (UnknownHostException e) {
				throw new RuntimeException(e);
			}
		}
		return s;
	}
}
