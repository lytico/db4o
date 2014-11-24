/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.foundation;

import java.util.*;

import com.db4o.foundation.*;

import db4ounit.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class Collections4TestCase implements TestCase {
	
	public void testSequenceSort() {
		assertSequenceSort(3, 2, 1 );
	}

	private void assertSequenceSort(Object... elements) {
		final Collection4 sequence = new Collection4(elements);
		Collections4.sort(sequence, new Comparison4() {
			public int compare(Object x, Object y) {
				return ((Comparable)x).compareTo(y);
			}
		});
		Arrays.sort(elements);
		Iterator4Assert.areEqual(Iterators.iterate(elements), sequence.iterator());
	}

}
