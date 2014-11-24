/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.ta.instrumentation.test.collections;

import java.util.*;

public class MyHashMap extends HashMap {

	public Map _delegate;
	
	public MyHashMap() {
		_delegate = new HashMap();
	}
	
	public Object put(Object key, Object value) {
		_delegate.put(key, value);
		return super.put(key, value);
	}
	
}
