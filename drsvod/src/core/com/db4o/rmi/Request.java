package com.db4o.rmi;

import java.io.*;
import java.lang.annotation.*;
import java.lang.reflect.*;
import java.util.*;

@SuppressWarnings({ "rawtypes", "unchecked" })
class Request {

	private volatile Object value;
	private volatile boolean done = false;
	private Method method;
	private Object[] args;
	private List<Callback<?>> callbacks;
	private final Distributor<?> distributor;
	private Object object;

	public Request(Distributor<?> distributor, Method method, Object[] args) {
		this.distributor = distributor;
		this.method = method;
		this.args = args;
	}

	public Request(Distributor<?> distributor, Object object) {
		this.distributor = distributor;
		this.object = object;
	}

	public synchronized Object get() throws InterruptedException {
		if (!done) {
			wait();
		}
		return value;
	}

	public Object getResult() {
		return value;
	}

    public synchronized void set(Object value) {
		this.value = value;
		done = true;
		notifyAll();
		if (callbacks != null) {
			for (Callback callback : callbacks) {
				callback.returned(value);
			}
			callbacks = null;
		}
	}

    public synchronized void addCallback(Callback callback) {
		if (done) {
			callback.returned(value);
			return;
		}
		if (callbacks == null) {
			callbacks = new ArrayList<Callback<?>>();
		}
		callbacks.add(callback);
	}

	public void serialize(DataOutput out) throws IOException {

		out.writeUTF(method.getName());
		Class<?>[] paramTypes = method.getParameterTypes();
		out.writeByte(paramTypes.length);

		for (int i = 0; i < paramTypes.length; i++) {
			Class<?> t = paramTypes[i];
			out.writeUTF(t.getName());
			Object o = args[i];
			if (o == null) {
				out.writeBoolean(false);
				continue;
			}
			out.writeBoolean(true);

			Annotation[] anns = method.getParameterAnnotations()[i];
			
			boolean proxy = Distributor.hasAnnotation(anns, Proxy.class);

			out.writeBoolean(proxy);
			if (proxy) {
				
				out.writeBoolean(Distributor.hasAnnotation(anns, Async.class));

				ServerObject server = distributor.serverFor(o);

				out.writeLong(server.getId());
				out.writeUTF(t.getName());

			} else {

				distributor.serializerFor(t).serialize(out, o);
			}
		}
	}
	
	public void deserialize(DataInput in) throws IOException {

		String name = in.readUTF();
		Class<?>[] paramTypes = new Class<?>[in.readByte()];
		args = new Object[paramTypes.length];

		for (int i = 0; i < paramTypes.length; i++) {

			Class<?> t = distributor.classForName(in.readUTF());
			paramTypes[i] = t;

			if (!in.readBoolean()) {
				args[i] = null;
				continue;
			}

			boolean proxy = in.readBoolean();

			if (proxy) {

				boolean async = in.readBoolean();
				
				long id = in.readLong();
				Class<?> clazz = distributor.classForName(in.readUTF());

				ProxyObject<?> peer = distributor.proxyFor(id, clazz);
				args[i] = async ? peer.async() : peer.sync();

			} else {

				args[i] = distributor.serializerFor(t).deserialize(in);
			}
		}
		resolveMethod(name, paramTypes);
	}

	private void resolveMethod(String name, Class<?>[] paramTypes) {
		try {
			method = object.getClass().getMethod(name, paramTypes);
			method.setAccessible(true);
		} catch (SecurityException e) {
			throw new RuntimeException(e);
		} catch (NoSuchMethodException e) {
			throw new RuntimeException(e);
		}
	}

	public void invoke() {
		try {
			value = method.invoke(object, args);
		} catch (IllegalArgumentException e) {
			throw new RuntimeException(e);
		} catch (IllegalAccessException e) {
			throw new RuntimeException(e);
		} catch (InvocationTargetException e) {
			throw new RuntimeException(e);
		}
	}

	public boolean hasValue() {
		return done;
	}

    public Class<?> getResultDeclaredType() {
        return method.getReturnType();
    }

}
