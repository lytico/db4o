/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test.collections;

import java.util.*;

public class HashtableFactory {
	
	public Map createHashtable() {
		return new Hashtable();
	}

	public Map createHashtableWithSize() {
		return new Hashtable(42);
	}

	public Map createHashtableWithSizeAndLoad() {
		return new Hashtable(42, (float)0.5);
	}
	
	public Map createHashtableFromMap() {
		return new Hashtable(new Hashtable());
	}

}
