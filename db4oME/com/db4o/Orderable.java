/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

interface Orderable {
	
	int compareTo(Object obj);
	void hintOrder(int a_order, boolean a_major);
	boolean hasDuplicates();
	
}

