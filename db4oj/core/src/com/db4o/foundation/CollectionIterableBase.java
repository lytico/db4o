/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */
package com.db4o.foundation;

import java.util.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class CollectionIterableBase implements IterableBaseWrapper {

	private Collection _delegate;
	
	public CollectionIterableBase(Collection delegate) {
		_delegate = delegate;
	}
	
	public Iterator iterator() {
		return _delegate.iterator();
	}

	public Object delegate() {
		return _delegate;
	}

}
