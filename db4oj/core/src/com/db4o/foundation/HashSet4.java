/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.foundation;

public class HashSet4 implements Set4 {

	private Hashtable4 _map;
	
	public HashSet4() {
		this(1);
	}
	
	public HashSet4(int count) {
		_map = new Hashtable4(count);
	}
	
	public boolean add(Object obj) {
		if(_map.containsKey(obj)) {
			return false;
		}
		_map.put(obj, obj);
		return true;
	}

	public void clear() {
		_map.clear();
	}

	public boolean contains(Object obj) {
		return _map.containsKey(obj);
	}

	public boolean isEmpty() {
		return _map.size() == 0;
	}

	public Iterator4 iterator() {
		return _map.values().iterator();
	}

	public boolean remove(Object obj) {
		return _map.remove(obj) != null;
	}

	public int size() {
		return _map.size();
	}
	
	public String toString() {
		return Iterators.join(_map.keys() , "{", "}", ", ");
	}


}
