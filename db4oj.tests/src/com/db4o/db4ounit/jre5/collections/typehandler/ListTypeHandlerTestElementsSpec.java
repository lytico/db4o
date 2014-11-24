/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre5.collections.typehandler;

import db4ounit.fixtures.*;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ListTypeHandlerTestElementsSpec implements Labeled {

	public final Object[] _elements;
	public final Object _notContained;
	public final Object _largeElement;
	
	public ListTypeHandlerTestElementsSpec(Object[] elements, Object notContained, Object largeElement) {
		_elements = elements;
		_notContained = notContained;
		_largeElement = largeElement;
	}

	public String label() {
		return _elements[0].getClass().getSimpleName();
	}
}
