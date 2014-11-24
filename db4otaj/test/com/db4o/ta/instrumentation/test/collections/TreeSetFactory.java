/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test.collections;

import java.util.*;

public class TreeSetFactory {
	
	public Set createTreeSet(){
		return new TreeSet();
	}
	
	public Set createTreeSetWithComparator(){
		return new TreeSet(new Comparator() {
			public int compare(Object o1, Object o2) {
				return 0;
			}
		});
	}
	
	public Set createTreeSetFromCollection(){
		return new TreeSet(new ArrayList());
	}
	
	public Set createTreeSetFromSortedSet(){
		return new TreeSet(new TreeSet());
	}
	
	

}
