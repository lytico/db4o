/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test.data;

import java.util.*;

public class TreeSetClient implements CollectionClient{

	private Set _set;
	
	public TreeSetClient() {
		_set = new TreeSet();
	}

	public Object collectionInstance() {
		return _set;
	}

}
