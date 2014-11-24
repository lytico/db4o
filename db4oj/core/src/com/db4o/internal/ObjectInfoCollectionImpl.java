/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.ext.*;
import com.db4o.foundation.*;

/**
 * @exclude
 */
public final class ObjectInfoCollectionImpl implements ObjectInfoCollection {
	
	public static final ObjectInfoCollection EMPTY = new ObjectInfoCollectionImpl(Iterators.EMPTY_ITERABLE);
	
	public Iterable4 _collection;

	public ObjectInfoCollectionImpl(Iterable4 collection) {
		_collection = collection;
	}

	public Iterator4 iterator() {
		return _collection.iterator();
	}
}