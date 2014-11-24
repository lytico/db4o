/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.ta.instrumentation.test.collections;

import java.util.*;

public class HashMapFactory {

	public Map createHashMap() {
		return new HashMap();
	}

	public Map createHashMapWithSize() {
		return new HashMap(42);
	}

	public Map createHashMapWithSizeAndLoad() {
		return new HashMap(42, (float)0.5);
	}
	
	public Map createHashMapFromMap() {
		return new HashMap(new HashMap());
	}


}
