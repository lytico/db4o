/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */

package com.db4o.collections;

import java.util.*;

import com.db4o.ta.*;

/**
 * @sharpen.ignore
 */
@decaf.Remove(decaf.Platform.JDK11)
public final class DelegatingActivatableSet<K> extends DelegatingSet<K> {
	private final Activatable _activatable;

	public DelegatingActivatableSet(Activatable activatable, Set<K> delegating) {
		super(delegating);
		_activatable = activatable;			
	}

	@Override
	public Iterator<K> iterator() {
		return new ActivatingIterator<K>(_activatable, super.iterator());
	}
}