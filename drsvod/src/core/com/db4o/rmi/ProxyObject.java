package com.db4o.rmi;

import java.lang.reflect.*;

public class ProxyObject<T> implements Peer<T> {

	private T syncFacade;

	private T asyncFacade;

	private Class<T> facade;

	private final Distributor<?> distributor;
	private final long id;

	ProxyObject(Distributor<?> distributor, long id, Class<T> facade) {
		this.id = id;
		this.facade = facade;
		this.distributor = distributor;
	}

	@SuppressWarnings("unchecked")
    public T sync() {
		if (syncFacade == null) {
			syncFacade = (T) java.lang.reflect.Proxy.newProxyInstance(getClass().getClassLoader(), new Class<?>[] { facade }, new InvocationHandler() {
				public Object invoke(Object proxy, Method method, Object[] args) throws Throwable {
					// TODO: Consider implementing #hashCode() in the same way.
					if(method.getName().equals("equals")){
						Object other = args[0];
						return other == syncFacade || other == asyncFacade;
					}else{
						Request r = distributor.request(getId(), method, args, true);
						if (!r.hasValue()) {
							distributor.feed();
						}
						return r.get();
					}
				}
			});
		}
		return syncFacade;
	}

	public T async() {
		if (asyncFacade == null) {
			asyncFacade = async(null);
		}
		return asyncFacade;
	}

	@SuppressWarnings("unchecked")
    public <R> T async(final Callback<R> callback) {
		return (T) java.lang.reflect.Proxy.newProxyInstance(getClass().getClassLoader(), new Class<?>[] { facade }, new InvocationHandler() {

			public Object invoke(Object proxy, Method method, Object[] args) throws Throwable {
				Request r = distributor.request(getId(), method, args, callback != null);
				if (callback != null) {
					if (!r.hasValue()) {
						distributor.feed();
					}
					r.addCallback(callback);
				}
				Class<?> type = method.getReturnType();
				if (type.isPrimitive()) {
					return (type == Boolean.class || type == Boolean.TYPE) ? false : 0;
				}
				return null;
			}
		});
	}

	public long getId() {
		return id;
	}

}
