/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.ta.instrumentation.test.data;

import java.util.*;

public class ArrayListClient implements CollectionClient {

	private List _list;
	
	public ArrayListClient() {
		_list = new ArrayList();
	}

	public Object collectionInstance() {
		return _list;
	}
}
