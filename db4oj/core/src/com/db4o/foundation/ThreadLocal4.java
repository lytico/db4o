/**
 * 
 */
package com.db4o.foundation;

import java.util.*;

/**
 * ThreadLocal implementation for less capable platforms such as JRE 1.1 and
 * Silverlight.
 * 
 * This class is not intended to be used directly, use {@link DynamicVariable}.
 * 
 * WARNING: This implementation might leak Thread references unless
 * {@link #set(Object)} is called with null on the right thread to clean it up. This
 * behavior is currently guaranteed by {@link DynamicVariable}.
 */
public class ThreadLocal4<T> {
	
	private final Map<Thread, T> _values = new HashMap<Thread, T>();
	
	public synchronized void set(T value) {
		if (value == null) {
			_values.remove(Thread.currentThread());
		} else {
			_values.put(Thread.currentThread(), value);
		}
    }
	
	public synchronized T get() {
		return _values.get(Thread.currentThread());
	}

	protected final T initialValue() {
	    return null;
    }
}