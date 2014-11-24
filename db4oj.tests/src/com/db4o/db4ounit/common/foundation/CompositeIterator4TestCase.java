/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.foundation;

import java.util.*;

import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CompositeIterator4TestCase implements TestCase {

	public void testWithEmptyIterators() {		
		assertIterator(newIterator());	
	}
	
	public void testReset() {
		CompositeIterator4 iterator = newIterator();
		assertIterator(iterator);
		iterator.reset();
		assertIterator(iterator);
	}

	private void assertIterator(final CompositeIterator4 iterator) {
		Iterator4Assert.areEqual(IntArrays4.newIterator(new int[] { 1, 2, 3, 4, 5, 6 }), iterator);
	}

	private CompositeIterator4 newIterator() {
		Collection4 iterators = new Collection4();
		iterators.add(IntArrays4.newIterator(new int[] { 1, 2, 3 }));
		iterators.add(IntArrays4.newIterator(new int[] { }));
		iterators.add(IntArrays4.newIterator(new int[] { 4 }));
		iterators.add(IntArrays4.newIterator(new int[] { 5, 6 }));
		
		final CompositeIterator4 iterator = new CompositeIterator4(iterators.iterator());
		return iterator;
	}
	
	public void testRecursionFree() {
		List<Iterator4<Object>> list = new ArrayList<Iterator4<Object>>();
		Iterator4<Object> emptyIterator = new Iterator4<Object>() {
			
			@Override
			public void reset() {
				throw new java.lang.UnsupportedOperationException();
			}
			
			@Override
			public boolean moveNext() {
				return false;
			}
			
			@Override
			public Object current() {
				throw new java.lang.UnsupportedOperationException();
			}
		};
		for(int i=0;i<100;i++) {
			list.add(emptyIterator);
		}
		
		Iterator4 ci = new CompositeIterator4(list.toArray(new Iterator4[list.size()])) {
			boolean recursion = false;
			@Override
			public boolean moveNext() {
				if (recursion) {
					Assert.fail("Recursion in moveNext is not allowed");
				}
				recursion = true;
				try {
					return super.moveNext();
				} finally {
					recursion = false;
				}
			}
		};
		while(ci.moveNext()) {
			// make .Net happy
			Object current = ci.current();
		}
	}

	
}
