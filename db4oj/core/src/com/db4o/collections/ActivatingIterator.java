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
public class ActivatingIterator<E> implements Iterator<E> {

	private final Activatable _activatable;
	protected final Iterator<E> _iterator;
	
	public ActivatingIterator(Activatable activatable, Iterator<E> iterator) {
		_activatable = activatable;
		_iterator = iterator;
	}
	
	public boolean hasNext() {
		activate(ActivationPurpose.READ);
		return _iterator.hasNext();
	}

	public E next() {
		activate(ActivationPurpose.READ);
		return _iterator.next();
	}

	public void remove() {
		activate(ActivationPurpose.WRITE);
		_iterator.remove();
	}
	
	protected void activate(ActivationPurpose purpose) {
		_activatable.activate(purpose);
	}
}
