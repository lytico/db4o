/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test.data;

import java.util.*;

public class StackClient implements CollectionClient  {

	private List _stack;
	
	public StackClient() {
		_stack = new Stack();
	}

	public Object collectionInstance() {
		return _stack;
	}

}
