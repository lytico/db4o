/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.fixtures;

import com.db4o.foundation.*;

public class SimpleFixtureProvider<T> implements FixtureProvider {

	private final FixtureVariable<T> _variable;
	private final T[] _values;

	public SimpleFixtureProvider(FixtureVariable<T> variable, T... values) {
		_variable = variable;
		_values = values;
	}

	public FixtureVariable<T> variable() {
		return _variable;
	}

	public Iterator4 iterator() {
		return Iterators.iterate(_values);
	}	
}
