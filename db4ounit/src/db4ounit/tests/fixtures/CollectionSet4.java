package db4ounit.tests.fixtures;

import java.util.*;

public class CollectionSet4 implements Set4 {
	
	private Vector _vector = new Vector();

	public void add(Object value) {
		_vector.addElement(value);
	}

	public boolean contains(Object value) {
		return _vector.contains(value);
	}
	
	public int size() {
		return _vector.size();
	}
}
