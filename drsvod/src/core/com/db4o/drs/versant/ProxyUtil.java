package com.db4o.drs.versant;

import java.lang.reflect.*;
import java.util.*;

public class ProxyUtil {


	public static final class InvocationHandlerImplementation<T, E extends T> implements InvocationHandler {
		private final E object;
		private final Class<? extends Object> clazz;
		volatile boolean in = false;
		Object lock = new Object();
		private Set<Thread> threads = new LinkedHashSet<Thread>();
		private List<StackTraceElement[]> store = new ArrayList<StackTraceElement[]>();
		
		private InvocationHandlerImplementation(E object) {
			this.object = object;
			this.clazz = object.getClass();
		}

		public Object invoke(Object proxy, Method method, Object[] args) throws Throwable {
			if ("store".equals(method.getName())) {
				store.add(Thread.currentThread().getStackTrace());
			} else if ("commit".equals(method.getName())) {
				store.clear();
			} else if ("close".equals(method.getName())) {
				if (!store.isEmpty()) {
					for (StackTraceElement[] stes : store) {
						System.out.println("store");
						for (StackTraceElement ste : stes) {
							System.out.println("    " + ste);
						}
					}
					throw new IllegalStateException();
				}
			}
			System.err.println("---> " + clazz.getSimpleName()+"."+method.getName() + " ("+System.identityHashCode(object)+", "+Thread.currentThread().getName()+")");
			synchronized (lock) {
				accessedFrom(Thread.currentThread());
				if (in) {
					throw new IllegalStateException("ha!");
				}
				in = true;
			}
			try {
				return method.invoke(object, args);
			} catch (InvocationTargetException e) {
				throw e.getCause();
			} finally {
				in = false;
			}
		}

		private void accessedFrom(Thread currentThread) {
			if (threads.add(currentThread)) {
				System.err.println("----------> new thread accessing " + clazz.getSimpleName());
				for (Thread t : threads) {
					System.err.println("     -> " + t.getName());
				}
			}
		}

	}

	public static <T, E extends T> T sync(final E object) {

		Class<? extends Object> clazz = object.getClass();
		return (T) Proxy.newProxyInstance(clazz.getClassLoader(), clazz.getInterfaces(), new InvocationHandler() {

			public Object invoke(Object proxy, Method method, Object[] args) throws Throwable {
				synchronized (object) {
					try { 
						return method.invoke(object, args);
					} catch (InvocationTargetException e) {
						throw e.getCause();
					}
				}
			}
		});
	}

	public static <T, E extends T> T noop(final E object) {
		return object;
	}

	public static <T, E extends T> T trace(final E object) {

		
		Class<? extends Object> clazz = object.getClass();
		return (T) Proxy.newProxyInstance(clazz.getClassLoader(), clazz.getInterfaces(), new InvocationHandler() {

			private static final String SEP = "    ";

			volatile boolean in = false;
			Object lock = new Object();
			

			String ident = "";
			public Object invoke(Object proxy, Method method, Object[] args) throws Throwable {
				String m = object.getClass().getSimpleName() + "."+method.getName() +"()";
//				String m = iface.getSimpleName()+"#"+method.getName();
				try {
					System.out.println(ident+m+" { # " + Thread.currentThread().getName());
					ident += SEP;
					
					synchronized (lock) {
						if (in) {
							throw new IllegalStateException("ha!");
						}
						in = true;
					}

					try {
					
						return method.invoke(object, args);
					} finally {
						in = false;
					}
				} catch (InvocationTargetException e) {
					throw e.getCause();
				} finally {
					ident = ident.substring(0, ident.length()-SEP.length());
					System.out.println(ident + "}");
				}
			}
		});
	}

	public static <T, E extends T> T throwOnConcurrentAccess(final E object) {

		Class<? extends Object> clazz = object.getClass();
		return (T) Proxy.newProxyInstance(clazz.getClassLoader(), clazz.getInterfaces(), new InvocationHandler() {

			volatile boolean in = false;
			Object lock = new Object();

			public Object invoke(Object proxy, Method method, Object[] args) throws Throwable {
				synchronized (lock) {
					if (in) {
						throw new IllegalStateException("ha!");
					}
					in = true;
				}
				try {
					return method.invoke(object, args);
				} catch (InvocationTargetException e) {
					throw e.getCause();
				} finally {
					in = false;
				}
			}

		});
	}
	
	public static <T, E extends T> T throwOnConcurrentAccess2(final E object) {

		Class<? extends Object> clazz = object.getClass();
		System.err.println("---> " + clazz.getSimpleName()+" created ("+System.identityHashCode(object)+", "+Thread.currentThread().getName()+")");
		
		return (T) Proxy.newProxyInstance(clazz.getClassLoader(), clazz.getInterfaces(), new InvocationHandlerImplementation(object));
	}

	public static <T> T threadLocal(Class<T> iface, final ThreadLocal<T> local) {
		return (T) Proxy.newProxyInstance(iface.getClassLoader(), new Class<?>[]{iface}, new InvocationHandler() {
			public Object invoke(Object proxy, Method method, Object[] args) throws Throwable {
				try {
					return method.invoke(local.get(), args);
				} catch (InvocationTargetException e) {
					throw e.getCause();
				}
			}
		});
	}

}
