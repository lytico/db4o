package com.db4o.drs.versant.ipc.tcp;

import java.io.*;
import java.net.*;
import java.util.*;

import com.db4o.drs.versant.ipc.*;
import com.db4o.internal.*;
import com.db4o.rmi.*;

public class TcpServer implements ServerChannelControl {

	private volatile ServerSocket server;
	private Set<Dispatcher> dispatchers = new HashSet<Dispatcher>();

	private final EventProcessor provider;

	private Thread serverThread;
	private boolean normalStop;
	private int port;

	public TcpServer(EventProcessor provider, int port) {

		this.provider = provider;
		this.port = port;

		serverThread = new Thread(ReflectPlatform.simpleName(EventProcessor.class) + " channel tcp server") {
			@Override
			public void run() {
				runServer();
			}
		};
		serverThread.setDaemon(true);
		
		serverThread.start();
		
		waitForServerReady();

	}

	public void stop() {
		normalStop = true;
		stopServer();
		stopDispatchers();
	}

	private void stopServer() {
		ServerSocket s = server;
		server = null;
		if (s != null) {
			try {
				s.close();
			} catch (IOException e) {
				reportException(e);
			}
		}
	}

	private void reportException(Throwable e) {
		if (normalStop) {
			return;
		}
		System.err.println("Exception thrown in TcpServer. Thread: " + Thread.currentThread().getName());
		e.printStackTrace();
	}

	private void stopDispatchers() {
		List<Dispatcher> cs = new ArrayList<Dispatcher>();
		synchronized (dispatchers) {
			cs.addAll(dispatchers);
		}
		for (Dispatcher socket : cs) {
			try {
				socket.close();
			} catch (IOException e) {
				reportException(e);
			}
		}
	}

	private synchronized void waitForServerReady() {
		if (server == null) {
			try {
				wait();
			} catch (InterruptedException e) {
				reportException(e);
			}
		}
	}
	
	public void join() throws InterruptedException {
		serverThread.join();
		ArrayList<Dispatcher> d;
		synchronized (dispatchers) {
			d = new ArrayList<Dispatcher>(dispatchers);
		}
		for (Dispatcher dispatcher : d) {
			dispatcher.join();
		}
	}

	private void runServer0() throws IOException, SocketException {
		
		ServerSocket localServer;
		synchronized (this) {
			server = new ServerSocket(port, 100);
			server.setReuseAddress(true);
			notifyAll();
			localServer = server;
		}

		while (true) {
			Socket socket = localServer.accept();
			synchronized (dispatchers) {
				if (server == null) {
					break;
				}
				dispatchers.add(new Dispatcher(socket));
			}
		}

	}

	private void runServer() {
		try {
			runServer0();
		} catch (IOException e) {
			reportException(e);
		} finally {
			synchronized (this) {
				notifyAll();
			}
		}
	}

	public class Dispatcher implements Runnable {

		private final Socket client;
		private Thread thread;

		public Dispatcher(Socket socket) {
			this.client = socket;
			thread = new Thread(this, ReflectPlatform.simpleName(EventProcessor.class)+" dispatcher for socket: " + socket);
			thread.setDaemon(true);
			thread.start();
		}

		public void join() throws InterruptedException {
			if (Thread.currentThread() != thread) {
				thread.join();
			}
		}

		public void close() throws IOException {
			client.close();
			try {
				join();
			} catch (InterruptedException e) {
				reportException(e);
			}
		}

		public void run() {
			try {
				DataInputStream in = new DataInputStream(new BufferedInputStream(client.getInputStream()));
				final DataOutputStream out = new DataOutputStream(new BufferedOutputStream(client.getOutputStream()));

				ByteArrayConsumer outgoingConsumer = new ByteArrayConsumer() {

					public void consume(byte[] buffer, int offset, int length) throws IOException {
						out.writeInt(length);
						out.write(buffer, offset, length);
						out.flush();
					}
				};
				Distributor<EventProcessor> localPeer = new Distributor<EventProcessor>(outgoingConsumer, provider);
				while (true) {
					TcpCommunicationNetwork.feed(in, localPeer);
				}
			} catch (IOException e) {
			} finally {
				synchronized (dispatchers) {
					dispatchers.remove(this);
				}
			}
		}

	}

}
