/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */
package com.db4o.foundation;

import java.lang.reflect.*;
import java.util.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class ReflectionIterableBase implements IterableBaseWrapper {

	private Object _delegate;
	private Method _method;
	
	public ReflectionIterableBase(Object delegate) throws Exception {
		_delegate = delegate;
		_method = _delegate.getClass().getMethod("iterator", new Class[0]);
		_method.setAccessible(true);
	}
	
	public Iterator iterator() {
		try {
			return (Iterator) _method.invoke(_delegate, new Object[0]);
		} catch (Exception exc) {
			exc.printStackTrace();
			return null;
		}
	}

	public Object delegate() {
		return _delegate;
	}

}
