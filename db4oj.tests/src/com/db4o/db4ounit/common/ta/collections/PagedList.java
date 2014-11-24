/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.ta.collections;

import java.util.*;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;

/**
 * Platform specific facade.
 * 
 * @param 
 * 
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class PagedList extends /* TA BEGIN */ ActivatableImpl /* TA END */ implements List {
		
	PagedBackingStore _store = new PagedBackingStore();
	
	public PagedList() {

	}

	public boolean add(Object item) {
		// TA BEGIN
		activate(ActivationPurpose.READ);
		// TA END
		return _store.add(item);
	}
	
	public Object get(int index) {
		// TA BEGIN
		activate(ActivationPurpose.READ);
		// TA END
		return _store.get(index);
	}

	
	public int size() {
		// TA BEGIN
		activate(ActivationPurpose.READ);
		// TA END
		return _store.size();
	}

	public void add(int index, Object element) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public boolean addAll(Collection c) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public boolean addAll(int index, Collection c) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public void clear() {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public boolean contains(Object o) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public boolean containsAll(Collection c) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public int indexOf(Object o) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public boolean isEmpty() {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public Iterator iterator() {
		// TA BEGIN
		activate(ActivationPurpose.READ);
		// TA END
		return new SimpleListIterator(this);
	}

	public int lastIndexOf(Object o) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public ListIterator listIterator() {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public ListIterator listIterator(int index) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public boolean remove(Object o) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public Object remove(int index) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public boolean removeAll(Collection c) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public boolean retainAll(Collection c) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public Object set(int index, Object element) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public List subList(int fromIndex, int toIndex) {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public Object[] toArray() {
		throw new com.db4o.foundation.NotImplementedException();
	}

	public Object[] toArray(Object[] a) {
		throw new com.db4o.foundation.NotImplementedException();
	}
}