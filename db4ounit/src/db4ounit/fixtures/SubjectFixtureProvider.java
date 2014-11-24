/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.fixtures;

import com.db4o.foundation.*;

public class SubjectFixtureProvider implements FixtureProvider {
	
	public static <T> T value() {
		return (T)_variable.value();
	}
	
	private static final FixtureVariable _variable = new FixtureVariable("subject");
	private final Iterable4 _values;
	
	public SubjectFixtureProvider(Iterable4 values) {
		_values = values;
	}
	
	public <T> SubjectFixtureProvider(T... values) {
		this(Iterators.iterable(values));
	}

	public FixtureVariable variable() {
		return _variable;
	}

	public Iterator4 iterator() {
		return _values.iterator();
	}
}
