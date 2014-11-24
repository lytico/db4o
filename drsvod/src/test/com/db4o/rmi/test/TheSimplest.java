package com.db4o.rmi.test;

import java.io.*;
import java.nio.channels.*;
import java.util.*;
import java.util.Map.Entry;
import java.util.concurrent.*;
import java.util.concurrent.atomic.*;

import junit.framework.*;

import com.db4o.rmi.*;

public class TheSimplest extends TestCase {
    
    public static class CustomClass {
        public final int opaque;

        public CustomClass(int opaque) {
            this.opaque = opaque;
        }
        
        public static final Serializer<CustomClass> customClassSerializer = new Serializer<CustomClass>() {
            
            public void serialize(DataOutput out, CustomClass value) throws IOException {
                out.writeInt(value.opaque);
            }
            
            public CustomClass deserialize(DataInput in) throws IOException {
                return new CustomClass(in.readInt());
            }
        };
    }

	public static interface Facade {
		int intCall();

		void voidCall();

		void addListener(@Proxy Runnable r);
		
		boolean removeListener(@Proxy Runnable runnable);
		
		Map<String, Set<Integer>> collections(Map<String, List<Integer>> map);
		
		int customClassAsParameter(CustomClass cc);
		
		InputStream newByteArrayInputStream(byte[] content);
		
		InputStream input();
		
		OutputStream output();
		
		void incrementCounter(int delta);
        void waitUntilCounterIs(int value);
	}
	
	

	public static class FacadeImpl implements Facade {
		
		private List<Runnable> listeners = new ArrayList<Runnable>();
        private InputStream in;
        private OutputStream out;
        private AtomicInteger counter = new AtomicInteger(0);
        
        public FacadeImpl() {
            try {
                Pipe pipe = Pipe.open();
                out = Channels.newOutputStream(pipe.sink());
                in = Channels.newInputStream(pipe.source());
            } catch (IOException e) {
                throw new RuntimeException(e);
            }
        }

		public int intCall() {
			return 42;
		}

		public void voidCall() {
		}

		public void addListener(Runnable r) {
			synchronized (listeners) {
				listeners.add(r);
			}
		}

		public void triggerListeners() {
			List<Runnable> l;
			synchronized (listeners) {
				l = new ArrayList<Runnable>(listeners);
			}
			for (Runnable runnable : l) {
				runnable.run();
			}
		}

		public Map<String, Set<Integer>> collections(Map<String, List<Integer>> map) {
			Map<String, Set<Integer>> ret = new HashMap<String, Set<Integer>>();
			for (Entry<String, List<Integer>> entry : map.entrySet()) {
				ret.put(entry.getKey(), new HashSet<Integer>(entry.getValue()));
			}
			return ret;
		}

		public boolean removeListener(Runnable runnable) {
			synchronized (listeners) {
				return listeners.remove(runnable);
			}
		}

        public int customClassAsParameter(CustomClass cc) {
            return cc.opaque;
        }

        public InputStream newByteArrayInputStream(byte[] content) {
            return new ByteArrayInputStream(content);
        }

        public InputStream input() {
            return in;
        }

        public OutputStream output() {
            return out;
        }

        public void incrementCounter(int delta) {
            if (counter.addAndGet(delta) == 0) {
                synchronized (this) {
                    notify();
                }
            }
        }

        public void waitUntilCounterIs(int value) {
            if (counter.get() != value) {
                synchronized (this) {
                    if (counter.get() != value) {
                        try {
                            wait();
                        } catch (InterruptedException e) {
                            throw new RuntimeException();
                        }
                    }
                }
            }
        }
	}

	Distributor<Facade> client;
	Distributor<Facade> server;
	FacadeImpl concreteFacade;

	public void setUp() {

		concreteFacade = new FacadeImpl();

		server = new Distributor<Facade>(null, concreteFacade);
		client = new Distributor<Facade>(null, Facade.class);
		
        addCustomClassSerializer();
		
		server.setConsumer(client);
		client.setConsumer(server);
	}

    protected void addCustomClassSerializer() {
        server.addSerializer(CustomClass.customClassSerializer, CustomClass.class);
        client.addSerializer(CustomClass.customClassSerializer, CustomClass.class);
    }

	public void testBasic() throws InterruptedException {

		assertEquals(42, client.sync().intCall());

		final BlockingQueue<Integer> q = new LinkedBlockingQueue<Integer>();

		client.async(new Callback<Integer>() {

			public void returned(Integer value) {
				q.add(value);
			}
		}).intCall();

		assertEquals(42, (int) q.take());

		try {
			Thread.sleep(100);
		} catch (InterruptedException e) {
		    e.printStackTrace();
		}

		assertTrue(q.isEmpty());

		client.sync().voidCall();

	}

	public void testProxy() throws InterruptedException {
		final BlockingQueue<Object> q = new LinkedBlockingQueue<Object>();

		Runnable runnable = new Runnable() {
			public void run() {
				q.add(new Object());
			}
		};
		
		client.sync().addListener(runnable);

		assertTrue(q.isEmpty());

		concreteFacade.triggerListeners();

		assertNotNull(q.take());
		
		assertTrue(client.sync().removeListener(runnable));
	}
	
	public void testCollections() {
		Map<String, List<Integer>> map = new HashMap<String, List<Integer>>();
		
        map.put("1", new ArrayList<Integer>(Arrays.asList(new Integer[]{1,1,2})));
		
		Map<String, Set<Integer>> ret = client.sync().collections(map);
		
		assertEquals(1, ret.size());
		assertEquals(2, ret.values().iterator().next().size());
		
	}
	
	public void testCustomClass() {
	    assertEquals(42, client.sync().customClassAsParameter(new CustomClass(42)));
	}
	
	public void testInputStream() throws IOException {
	    InputStream in = client.sync().newByteArrayInputStream("here".getBytes());
	    
	    
	    byte[] buf = new byte[100];
	    int read = in.read(buf);
	    
	    assertEquals("here", new String(buf, 0, read));
	    assertEquals(-1, in.read());
	    in.close();
	    
	}

    public void testStreams() throws IOException {
        final DataOutputStream out = new DataOutputStream(new BufferedOutputStream(client.sync().output()));
        DataInputStream in = new DataInputStream(new BufferedInputStream(client.sync().input()));

        ExecutorService executor = Executors.newCachedThreadPool();
        executor.execute(new Runnable() {
            public void run() {
                try {
                    for (int i = 0; i < 100; i++) {
                        out.writeUTF("here: "+i);
                    }
                    out.flush();
                } catch (IOException e) {
                    throw new RuntimeException(e);
                }
            }
        });
        
        for(int i=0;i<100;i++) {
            assertEquals("here: "+i, in.readUTF());
        }
        
    }

    public void testConcurrencyStreams() throws IOException, InterruptedException {

        final Facade async = client.async();
        ExecutorService executor = Executors.newCachedThreadPool();
        for (int i=0;i<100;i++) {
            executor.execute(new Runnable() {
                public void run() {
                    for (int i = 0; i < 300; i++) {
                        async.incrementCounter(1);
                    }
                    for (int i = 0; i < 300; i++) {
                        async.incrementCounter(-1);
                    }
                }
            });
        }
        executor.shutdown();
        executor.awaitTermination(5, TimeUnit.SECONDS);
        
        client.sync().waitUntilCounterIs(0);
        
    }


}
