/* Copyright (C) 2011 Versant Inc.  http://www.db4o.com */
package com.db4o.drs.test;

import java.util.*;

import com.db4o.foundation.*;

/**
 * @sharpen.ignore
 */
public class DelegatingList implements List {

	protected List _delegate;

	public DelegatingList(List delegate) {
		_delegate = delegate;
	}

	public boolean add(Object o) {
		return _delegate.add(o);
	}

	public Iterator iterator() {
		return _delegate.iterator();
	}

	public void add(int index, Object element) {
		throw new NotImplementedException();
	}

	public boolean addAll(Collection c) {
		throw new NotImplementedException();
	}

	public boolean addAll(int index, Collection c) {
		throw new NotImplementedException();
	}

	public void clear() {
		throw new NotImplementedException();
	}

	public boolean contains(Object o) {
		throw new NotImplementedException();
	}

	public boolean containsAll(Collection c) {
		throw new NotImplementedException();
	}

	public Object get(int index) {
		throw new NotImplementedException();
	}

	public int indexOf(Object o) {
		throw new NotImplementedException();
	}

	public boolean isEmpty() {
		throw new NotImplementedException();
	}

	public int lastIndexOf(Object o) {
		throw new NotImplementedException();
	}

	public ListIterator listIterator() {
		throw new NotImplementedException();
	}

	public ListIterator listIterator(int index) {
		throw new NotImplementedException();
	}

	public boolean remove(Object o) {
		throw new NotImplementedException();
	}

	public Object remove(int index) {
		throw new NotImplementedException();
	}

	public boolean removeAll(Collection c) {
		throw new NotImplementedException();
	}

	public boolean retainAll(Collection c) {
		throw new NotImplementedException();
	}

	public Object set(int index, Object element) {
		throw new NotImplementedException();
	}

	public int size() {
		throw new NotImplementedException();
	}

	public List subList(int fromIndex, int toIndex) {
		throw new NotImplementedException();
	}

	public Object[] toArray() {
		throw new NotImplementedException();
	}

	public Object[] toArray(Object[] a) {
		throw new NotImplementedException();
	}

}