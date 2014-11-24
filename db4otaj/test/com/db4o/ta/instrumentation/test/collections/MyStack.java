/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
package com.db4o.ta.instrumentation.test.collections;

import java.util.*;

@SuppressWarnings("unchecked")
public class MyStack extends Stack {

	public Stack _delegate;
	
	public MyStack() {
		super();
		_delegate = new Stack();
	}

	@Override
	public Object push(Object item) {
		super.push(item);
		return _delegate.push(item);
	}	
}
