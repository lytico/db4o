/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

public class NoDuplicatesQueue implements Queue4 {

	private Queue4 _queue;
	private Hashtable4 _seen;
	
	public NoDuplicatesQueue(Queue4 queue) {
		_queue = queue;
		_seen = new Hashtable4();
	}
	
	public void add(Object obj) {
		if(_seen.containsKey(obj)) {
			return;
		}
		_queue.add(obj);
		_seen.put(obj, obj);
	}

	public boolean hasNext() {
		return _queue.hasNext();
	}

	public Iterator4 iterator() {
		return _queue.iterator();
	}

	public Object next() {
		return _queue.next();
	}

	public Object nextMatching(Predicate4 condition) {
		return _queue.nextMatching(condition);
	}

}
