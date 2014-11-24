package com.db4o.rmi;

import java.io.*;
import java.lang.annotation.*;
import java.lang.reflect.*;
import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.atomic.*;

public class Distributor<T> implements Peer<T>, ByteArrayConsumer {

	private static final int REQUEST = 0;
	private static final int RESPONSE = 1;

	private AtomicLong nextRequest = new AtomicLong(0);

	private ConcurrentMap<Long, Request> requests = new ConcurrentHashMap<Long, Request>();

	private Runnable feeder;

	private ByteArrayConsumer consumer;
	private ProxyObject<T> rootClient;
	private AtomicLong objectsId = new AtomicLong();
	
	private ConcurrentMap<Object, ServerObject> serving = new ConcurrentHashMap<Object, ServerObject>();
	private ConcurrentMap<Long, ServerObject> servingById = new ConcurrentHashMap<Long, ServerObject>();
	
	private volatile ConcurrentMap<Long, ProxyObject<?>> proxying = new ConcurrentHashMap<Long, ProxyObject<?>>();
    private Map<Class<?>, Serializer<?>> serializers = new HashMap<Class<?>, Serializer<?>>();

	public Distributor(ByteArrayConsumer consumer, Class<T> rootFacade) {
		this(consumer, null, rootFacade);
	}

	public Distributor(ByteArrayConsumer consumer, T root) {
		this(consumer, root, null);
	}

	private Distributor(ByteArrayConsumer consumer, T root, Class<T> rootFacade) {
	    addDefaultSerializers();
		setConsumer(consumer);

		if (root == null) {
			rootClient = proxyFor(objectsId.getAndIncrement(), rootFacade);
		} else {
			serverFor(root);
		}
	}

	private void addDefaultSerializers() {
        addSerializer(new InputStreamSerializer(this), InputStream.class);
        addSerializer(new OutputStreamSerializer(this), OutputStream.class);
    }


    private DataOutputStream newFlusher() {
		ByteArrayOutputStream bout = new ByteArrayOutputStream() {
			@Override
			public void flush() throws IOException {
				super.flush();
				Distributor.this.consumer.consume(this.buf, 0, this.count);
				reset();
			}
		};
		return new DataOutputStream(bout);
	}

	public void setConsumer(ByteArrayConsumer consumer) {
		this.consumer = consumer;
	}

	protected Request request(long objectId, Method method, Object[] args, boolean expectsResponse) throws IOException {
		Request r = new Request(this, method, args);
		long requestId = -1;
		
		if (expectsResponse) {
			requestId = nextRequest.getAndIncrement();
			requests.put(requestId, r);
		}

		sendRequest(objectId, requestId, r);

		return r;
	}

	public void consume(byte[] buffer, int offset, int length) throws IOException {
		DataInputStream in = new DataInputStream(new ByteArrayInputStream(buffer, offset, length));
		do {
			processOne(in);
			
		} while (in.readBoolean());
	}

	private void processOne(DataInputStream in) throws IOException {

		byte op = in.readByte();

		switch (op) {
		case REQUEST:
			processRequest(in);
			break;

		case RESPONSE:
			processResponse(in);
			break;

		default:
			throw new RuntimeException("Unknown operation: " + op);
		}

	}

	private void processRequest(DataInputStream in) throws IOException {

		long serverId = in.readLong();

		ServerObject server = servingById.get(serverId);

		long requestId = in.readLong();
		Request r = new Request(this, server.getObject());
		r.deserialize(in);
		r.invoke();
		if (requestId != -1) {
			sendResponse(requestId, r.getResult(), r.getResultDeclaredType());
		}
	}

    private void sendRequest(long objectId, long requestId, Request r) throws IOException {
		DataOutputStream out = newFlusher();
		out.writeByte(REQUEST);
		out.writeLong(objectId);
		out.writeLong(requestId);
		r.serialize(out);
		out.writeBoolean(false);
		out.flush();
	}

	private void sendResponse(long requestId, Object value, Class<?> declaredType) throws IOException {
        DataOutputStream out = newFlusher();
		out.writeByte(RESPONSE);
		out.writeLong(requestId);
		if (value == null) {
			out.writeBoolean(false);
		} else {
			out.writeBoolean(true);
			Class<?> clazz = value.getClass();
			out.writeUTF(clazz.getName());
			Serializer<Object> s = serializerFor(clazz, declaredType);
			s.serialize(out, value);
		}
		out.writeBoolean(false);
		out.flush();
	}

    protected Serializer<Object> serializerFor(Class<?> first, Class<?> second) {
        Serializer<Object> s = getSerializerFor(first);
        if (s != null) {
            return s;
        }
        if (second == null || first == second) {
            throw new RuntimeException("No serializer registered for " + first);
        }
        s = getSerializerFor(second);
        if (s != null) {
            return s;
        }
        throw new RuntimeException("No serializer registered for neither " + first + " or " + second);
    }

	private void processResponse(DataInputStream in) throws IOException {
		long requestId = in.readLong();
		Request r = requests.remove(requestId);

		if (r == null) {
			throw new IllegalStateException("Request " + requestId + " is unknown (last request generated was "  + nextRequest.get());
		}

		Object o = null;
		if (in.readBoolean()) {
			o = serializerFor(classForName(in.readUTF()), r.getResultDeclaredType()).deserialize(in);
		}
		r.set(o);
	}

	public T sync() {
		return rootClient.sync();
	}

	public T async() {
		return rootClient.async();
	}

	public <R> T async(final Callback<R> callback) {
		return rootClient.async(callback);
	}

	public void setFeeder(Runnable feeder) {
		this.feeder = feeder;
	}

	public Runnable getFeeder() {
		return feeder;
	}

	public ServerObject serverFor(Object o) {

		ServerObject server = serving.get(o);

		if (server == null) {

			server = new ServerObject(objectsId.getAndIncrement(), o);

			serving.put(o, server);
			servingById.put(server.getId(), server);
		}

		return server;
	}

	@SuppressWarnings("unchecked")
	public <S> ProxyObject<S> proxyFor(long id, Class<S> clazz) {

        ProxyObject<S> proxy = (ProxyObject<S>) proxying.get(id);

		if (proxy == null) {
		    
		    synchronized (proxying) {
		        proxy = (ProxyObject<S>) proxying.get(id);
		        if (proxy != null) {
		            return proxy;
		        }

    			proxy = new ProxyObject<S>(this, id, clazz);
    
    			proxying.put(proxy.getId(), proxy);
		    }
		}

		return proxy;
	}

	public void feed() {
		if (feeder == null) {
			return;
		}
		feeder.run();
	}
	
    public <R> Serializer<R> addSerializer(Serializer<R> serializer, Class<?>... classes) {
        for (Class<?> clazz : classes) {
            serializers.put(clazz, serializer);
        }
        return serializer;
    }
	
    public Serializer<Object> serializerFor(String className) {
        return serializerFor(classForName(className));
    }

    public Serializer<Object> serializerFor(Class<?> clazz) {
        return serializerFor(clazz, null);
    }

    @SuppressWarnings("unchecked")
    protected Serializer<Object> getSerializerFor(Class<?> clazz) {
        Serializer<Object> s = (Serializer<Object>) serializers.get(clazz);
        if (s != null) {
            return s;
        }
        return (Serializer<Object>) Serializers.serializerFor(clazz);
    }

    public static boolean hasAnnotation(Annotation[] anns, Class<? extends Annotation> clazz) {
        for (Annotation ann : anns) {
            if (clazz == ann.annotationType()) {
                return true;
            }
        }
        return false;
    }

    public Class<?> classForName(String className) {
        try {
            return ClassResolver.forName(className);
        } catch (ClassNotFoundException e) {
            throw new RuntimeException(e);
        }
    }

}
