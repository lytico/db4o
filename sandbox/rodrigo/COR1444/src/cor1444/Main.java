package cor1444;

import java.util.*;
import java.util.concurrent.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.*;
import com.db4o.cs.config.*;
import com.db4o.foundation.io.*;
import com.db4o.query.*;

import db4ounit.*;

public class Main {


	public static void main(String[] args) {
		new Main();
	}

	private static final String FILENAME = "cor1444.db4o";
	private static final int ITEM_COUNT = 100;
	private static final String FIELD_NAME = "_id";
	
	private final ExecutorService _pool = Executors.newCachedThreadPool();
	private final List<Class> _classes = Collections.synchronizedList(new ArrayList<Class>());
	private final CountDownLatch _startSignal = new CountDownLatch(1);
	private ObjectContainer _container;
	private ObjectServer _server;

	public Main() {
		createNewDatabase();
		try {
			executeConcurrentTasks();
		} finally {
			dispose();
		}
		dumpDatabaseInfo();
	}

	private void dumpDatabaseInfo() {
		openDatabase();
		try {
			final ObjectSet<Item> remaining = _container.query(Item.class);
			System.out.print("checking");
			for (Item item : remaining)
				System.out.print('.');
			System.out.println(" ok!");
			System.out.println(remaining.size() + " objects left in the database.");
		} finally {
			dispose();
		}
	}

	private void executeConcurrentTasks() {
		_pool.submit(new ClassEmitter());
		for (int i=0; i<3; ++i) {
			_pool.submit(new Producer());
			_pool.submit(new Consumer());
		}
		
		_startSignal.countDown();
		
		try {
			final int timeoutSeconds = 5;
			Thread.sleep(timeoutSeconds * 1000);
			_pool.shutdown();
			System.out.println("shutdown initiated...");
			final boolean finished = _pool.awaitTermination(timeoutSeconds, TimeUnit.SECONDS);
			System.out.println(finished ? "done" : "WARNING: still had work to do");
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
	}

	private void createNewDatabase() {
		File4.delete(FILENAME);
		openDatabase();
	}

	private void openDatabase() {
		final int port = 0xDB40;
		final String userName = "user";
		final String password = "password";
		
		final ServerConfiguration serverConfig = Db4oClientServer.newServerConfiguration();
		_server = Db4oClientServer.openServer(serverConfig, FILENAME, port);
		_server.grantAccess(userName, password);
		serverConfig.common().objectClass(Item1.class).objectField(FIELD_NAME).indexed(true);
		_container = Db4oClientServer.openClient(Db4oClientServer.newClientConfiguration(), "localhost", port, userName, password);
	}

	private void dispose() {
		_server.close();
		_container.close();
	}
	
	private final class ClassEmitter implements Runnable {
		public void run() {
			try {
				_classes.add(Item1.class);
				Thread.sleep(500);
				_classes.add(Item2.class);
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
		}
	}
	
	private final class Consumer extends Worker {

		@Override
		protected void doWork() {
			final Query query = _container.query();
			query.constrain(Item.class);
			final Query idField = query.descend(FIELD_NAME);
			idField.constrain(_begin).smaller().not();
			idField.constrain(_end).smaller();
			for (Object o : query.execute()) {
				final Item item = (Item)o;
				Assert.isSmaller(_end, item.id());
				_container.delete(item);
				System.out.println("CONSUMED: " + item);
			}
		}
		
	}

	private final class Producer extends Worker {
		@Override protected void doWork() {
			
			for (int i=_begin; i<_end; ++i) {
				final int id = i;
				_pool.submit(new Runnable() {
					public void run() {
						final Object item = newInstance(id);
						_container.store(item);
						System.out.println("PRODUCED: " + item);
					}
				});
			}
			_pool.submit(new Runnable() {
				public void run() {
					_container.commit();
				}
			});	
		}
	}

	abstract class Worker implements Runnable {
		
		protected Class _itemClass;
		protected int _begin;
		protected int _end;
		protected final Random _random = new Random();

		public void run() {
			try {
				waitForStartSignal();
				waitForClass();
				while (!_pool.isShutdown()) {
					_itemClass = _classes.get(_random.nextInt(_classes.size()));
					_end = _random.nextInt(ITEM_COUNT);
					_begin = _random.nextInt(_end);
					System.out.println(getClass() + " working from " + _begin + " to " + _end);
					doWork();
					Thread.sleep(100);
				}
			} catch (InterruptedException ex) {
				ex.printStackTrace();
			} 
		}

		private void waitForClass() {
			while (_classes.isEmpty() && !_pool.isShutdown())
				Thread.yield();
		}

		private void waitForStartSignal() throws InterruptedException {
			_startSignal.await();
		}
		
		protected Item newInstance(int id) {
			try {
				return (Item) _itemClass.getConstructor(Integer.TYPE).newInstance(id);
			} catch (Exception e) {
				throw new RuntimeException(e);
			}
		}

		protected abstract void doWork();
	}

}
