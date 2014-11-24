/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */

package com.db4o.collections;

import java.util.*;

/**
 * @sharpen.ignore
 */
@decaf.Remove(decaf.Platform.JDK11)
public class DelegatingSet<K> implements Set<K> {
	private final Set<K> _delegating;

	DelegatingSet(Set<K> originalSet) {
		_delegating = originalSet;
	}

	public boolean add(K e) {
		return _delegating.add(e);
	}

	public boolean addAll(Collection<? extends K> c) {
		return _delegating.addAll(c);
	}

	public void clear() {
		_delegating.clear();
	}

	public boolean contains(Object o) {
		return _delegating.contains(o);
	}

	public boolean containsAll(Collection<?> c) {
		return _delegating.containsAll(c);
	}

	public boolean equals(Object o) {
		return _delegating.equals(o);
	}

	public int hashCode() {
		return _delegating.hashCode();
	}

	public boolean isEmpty() {
		return _delegating.isEmpty();
	}

	public boolean remove(Object o) {
		return _delegating.remove(o);
	}

	public boolean removeAll(Collection<?> c) {
		return _delegating.removeAll(c);
	}

	public boolean retainAll(Collection<?> c) {
		return _delegating.retainAll(c);
	}

	public int size() {
		return _delegating.size();
	}

	public Object[] toArray() {
		return _delegating.toArray();
	}

	public <T> T[] toArray(T[] a) {
		return _delegating.toArray(a);
	}

	public Iterator<K> iterator() {
		return _delegating.iterator();
	}
}