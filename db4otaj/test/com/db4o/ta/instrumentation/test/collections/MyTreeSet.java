/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test.collections;

import java.util.*;

public class MyTreeSet extends TreeSet{
	
	public Set _delegate;
	
	public MyTreeSet(){
		_delegate = new TreeSet();
	}
	
	public boolean add(Object e) {
		_delegate.add(e);
		return super.add(e);
	}

}
