/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.fixtures;

import com.db4o.foundation.*;

import db4ounit.fixtures.FixtureContext.*;

public class FixtureVariable<T> {
	
	public static <T> FixtureVariable<T> newInstance(String label) {
		return new FixtureVariable<T>(label);
    }
	
	private final String _label;
	
	public FixtureVariable() {
		this("");
	}

	public FixtureVariable(String label) {
		_label = label;
	}
	
	/**
	 * @sharpen.property
	 */
	public String label() {
		return _label;
	}
	
	public String toString() {
		return _label;
	}
	
	public Object with(T value, Closure4 closure) {
		return inject(value).run(closure);
	}

	public void with(T value, Runnable runnable) {
		inject(value).run(runnable);
	}

	private FixtureContext inject(T value) {
		return currentContext().add(this, value);
	} 
	
	/**
	 * @sharpen.property
	 */
	public T value() {
		final Found found = currentContext().get(this);
		if (null == found) throw new IllegalStateException();
		return (T)found.value;
	}

	private FixtureContext currentContext() {
		return FixtureContext.current();
	}
}
