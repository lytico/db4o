/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test.collections;

import java.util.*;

public class MyHashtable extends Hashtable {

	public Map _delegate;
	
	public MyHashtable() {
		_delegate = new Hashtable();
	}
	
	public Object put(Object key, Object value) {
		_delegate.put(key, value);
		return super.put(key, value);
	}
	
}
