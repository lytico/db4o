/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;

/**
 * A dynamic variable is a value associated to a specific thread and scope.
 * 
 * The value is brought into scope with the {@link #with} method.
 * 
 */
public class DynamicVariable<T> {
	
	public static <T> DynamicVariable<T> newInstance() {
		return new DynamicVariable();
	}
	
	private final ThreadLocal<T> _value = new ThreadLocal<T>();
	
	/**
	 * @sharpen.property
	 */
	public T value() {
		final T value = _value.get();
		return value == null
			? defaultValue()
			: value;
	}
	
	/**
	 * @sharpen.property
	 */
	public void value(T value){
		_value.set(value);
	}
	
	protected T defaultValue() {
		return null;
	}
	
	public Object with(T value, Closure4 block) {
		T previous = _value.get();
		_value.set(value);
		try {
			return block.run();
		} finally {
			_value.set(previous);
		}
	}
	
	public void with(T value, Runnable block) {
		T previous = _value.get();
		_value.set(value);
		try {
			block.run();
		} finally {
			_value.set(previous);
		}
	}
}
