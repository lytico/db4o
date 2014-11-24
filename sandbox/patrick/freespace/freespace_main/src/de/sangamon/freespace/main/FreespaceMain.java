package de.sangamon.freespace.main;

import java.util.*;

import de.sangamon.freespace.*;
import de.sangamon.freespace.core.*;
import de.sangamon.freespace.managers.*;
import de.sangamon.freespace.actors.*;
import de.sangamon.freespace.actors.core.*;
import de.sangamon.freespace.actors.sync.*;
import de.sangamon.freespace.actors.react.*;

public class FreespaceMain {
	
	private static abstract class FreespaceManagerFactory {
		private String _name;
		
		public FreespaceManagerFactory(Class<?> clazz) {
			_name = clazz.getSimpleName();
		}
		
		public String name() {
			return _name;
		}

		public abstract FreespaceManager create();
	}
	
	private static final FreespaceManagerFactory[] MANAGERS = {
		new FreespaceManagerFactory(SynchronizedFreespaceManagerWrapper.class) {
			@Override
			public FreespaceManager create() {
				return new SynchronizedFreespaceManagerWrapper(createBaseManager());
			}
		},
		new FreespaceManagerFactory(QueueingFreespaceManager.class) {
			@Override
			public FreespaceManager create() {
				return new QueueingFreespaceManager(new SynchronizedFreespaceManagerWrapper(createBaseManager()));
			}
			
		},
		new FreespaceManagerFactory(SyncActorFreespaceManagerWrapper.class) {
			@Override
			public FreespaceManager create() {
				return new SyncActorFreespaceManagerWrapper();
			}
			
		},
		new FreespaceManagerFactory(ReactActorFreespaceManagerWrapper.class) {
			@Override
			public FreespaceManager create() {
				return new ReactActorFreespaceManagerWrapper();
			}
			
		},
	};

	private static FreespaceManager createBaseManager() {
		return new SimpleFreespaceManager();
	}
	
	public static void main(String[] args) {
		int mgrFromIdx = 0;
		int mgrToIdx = MANAGERS.length;
		int clientFromIdx = 0;
		int clientToIdx = FreespaceUtil.NUM_CLIENTS.length;
		if(args != null && args.length > 1) {
			mgrFromIdx = Integer.parseInt(args[0]);
			mgrToIdx = mgrFromIdx + 1;
			clientFromIdx = Integer.parseInt(args[1]);
			clientToIdx = clientFromIdx + 1;
		}
		for (int numClientsIdx = clientFromIdx; numClientsIdx < clientToIdx; numClientsIdx++) {
			System.out.print(',');
			System.out.print(FreespaceUtil.NUM_CLIENTS[numClientsIdx]);
		}
		System.out.println();
		for (int curIdx = mgrFromIdx; curIdx < mgrToIdx; curIdx++) {
			FreespaceManagerFactory factory = MANAGERS[curIdx];
			System.out.print(factory.name());
			for (int numClientsIdx = clientFromIdx; numClientsIdx < clientToIdx; numClientsIdx++) {
				System.out.print(',');
				FreespaceManager manager = factory.create();
				runManager(manager, FreespaceUtil.NUM_CLIENTS[numClientsIdx]);
			}
			System.out.println();
		}
	}

	private static void runManager(FreespaceManager manager, int numClients) {
		Set<Thread> threads = new HashSet<Thread>();
		for(int clientIdx = 0; clientIdx < numClients; clientIdx++) {
			FreespaceClient client = new FreespaceClient(clientIdx, FreespaceUtil.NUM_RUNS, FreespaceUtil.NUM_ACQUISITIONS, manager);
			threads.add(new Thread(client));
		}
		long start = System.currentTimeMillis();
		for(Thread thread : threads) {
			thread.start();
		}
		for(Thread thread : threads) {
			try {
				thread.join();
			}
			catch (InterruptedException exc) {
				exc.printStackTrace();
			}
		}
		manager.shutdown();
		long time = System.currentTimeMillis() - start;
		System.out.print(time);
	}

}
