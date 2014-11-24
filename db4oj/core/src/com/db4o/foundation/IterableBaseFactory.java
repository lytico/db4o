/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */
package com.db4o.foundation;

import java.util.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class IterableBaseFactory {

	/**
	 * @sharpen.unwrap
	 */
	public static IterableBase coerce(Object obj) {
		if(obj instanceof Collection) {
			return new CollectionIterableBase((Collection) obj);
		}
		try {
			return new ReflectionIterableBase(obj);
		}
		catch (Exception exc) {
			throw new RuntimeException(exc.getMessage());
		}
	}
	
	public static Object unwrap(IterableBase iterable) {
		if(iterable instanceof IterableBaseWrapper) {
			return ((IterableBaseWrapper)iterable).delegate();
		}
		return iterable;
	}
	
	private IterableBaseFactory() {
	}

}
