/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test.data;

import java.util.*;

public class HashtableClient implements CollectionClient  {
	
	private Map _map;
	
	public HashtableClient() {
		_map = new Hashtable();
	}
	
	public Object collectionInstance() {
		return _map;
	}

}
