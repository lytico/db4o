package db4ounit.tests.fixtures;

import java.util.*;

public class HashtableSet4 implements Set4 {
	
	Hashtable _table = new Hashtable();

	public void add(Object value) {
		_table.put(value, value);
	}

	public boolean contains(Object value) {
		return _table.containsKey(value);
	}

	public int size() {
		return _table.size();
	}
}
