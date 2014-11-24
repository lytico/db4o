/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;

import java.util.ArrayList;
import java.util.List;

public class NativeCollections {
	
	public static <T> List<T> filter(List<T> items, Predicate4<T> predicate) {
		
		List<T> filtered = new ArrayList<T>();
		for(T item : items) {
			if (predicate.match(item)) {
				filtered.add(item);
			}
		}
		
		return filtered;
	}

}
