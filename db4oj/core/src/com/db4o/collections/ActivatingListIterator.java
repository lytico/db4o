/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.collections;

import java.util.*;

import com.db4o.activation.*;
import com.db4o.ta.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Remove(decaf.Platform.JDK11)
public class ActivatingListIterator<E> extends ActivatingIterator<E> implements ListIterator<E> {

	public ActivatingListIterator(Activatable activatable, Iterator<E> iterator) {
		super(activatable, iterator);
	}

	public void add(E o) {
		activate(ActivationPurpose.WRITE);
		listIterator().add(o);
	}

	public boolean hasPrevious() {
		activate(ActivationPurpose.READ);
		return listIterator().hasPrevious();
	}

	public int nextIndex() {
		activate(ActivationPurpose.READ);
		return listIterator().nextIndex();
	}

	public E previous() {
		activate(ActivationPurpose.READ);
		return listIterator().previous();
	}

	public int previousIndex() {
		activate(ActivationPurpose.READ);
		return listIterator().previousIndex();
	}

	public void set(E o) {
		activate(ActivationPurpose.WRITE);
		listIterator().set(o);
	}
	
	private ListIterator<E> listIterator() {
		return (ListIterator<E>) _iterator;
	}
}
