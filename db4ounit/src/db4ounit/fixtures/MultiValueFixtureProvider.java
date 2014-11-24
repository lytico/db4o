/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.fixtures;

import com.db4o.foundation.*;

public class MultiValueFixtureProvider implements FixtureProvider {

	public static Object[] value() {
		return (Object[])_variable.value();
	}
	
	private static final FixtureVariable _variable = new FixtureVariable("data");
	
	private final Object[][] _values;

	public <T> MultiValueFixtureProvider(T[]... values) {
		_values = values;
	}

	public FixtureVariable variable() {
		return _variable;
	}

	public Iterator4 iterator() {
		return Iterators.iterate(_values);
	}
}
