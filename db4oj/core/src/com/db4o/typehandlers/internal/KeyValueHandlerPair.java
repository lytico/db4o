/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.typehandlers.internal;

import com.db4o.typehandlers.*;

/**
 * @exclude
 */
public class KeyValueHandlerPair {
	public final TypeHandler4 _keyHandler;
	public final TypeHandler4 _valueHandler;
	
	public KeyValueHandlerPair(TypeHandler4 keyHandler, TypeHandler4 valueHandler) {
		_keyHandler = keyHandler;
		_valueHandler = valueHandler;
	}
}