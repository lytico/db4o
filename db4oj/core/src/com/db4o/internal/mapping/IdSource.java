package com.db4o.internal.mapping;

import com.db4o.foundation.*;

public class IdSource {

	private final Queue4 _queue;

	public IdSource(Queue4 queue) {
		_queue = queue;
	}
	
	public boolean hasMoreIds() {
		return _queue.hasNext();
	}
	
	public int nextId() {
		return ((Integer)_queue.next()).intValue();
	}
}
