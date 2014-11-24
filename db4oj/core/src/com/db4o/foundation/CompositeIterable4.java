/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;


public class CompositeIterable4 implements Iterable4 {

	private final Iterable4 _iterables;

	public CompositeIterable4(Iterable4 iterables) {
		_iterables = iterables;
	}

	public Iterator4 iterator() {
		return new CompositeIterator4(_iterables.iterator()) {
			protected Iterator4 nextIterator(Object current) {
				return ((Iterable4)current).iterator();
			}
		};
	}

}
