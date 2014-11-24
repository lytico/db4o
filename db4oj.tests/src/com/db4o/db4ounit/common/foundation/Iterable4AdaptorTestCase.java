/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @exclude
 */
public class Iterable4AdaptorTestCase implements TestCase {
	
	public void testEmptyIterator() {
		final Iterable4Adaptor adaptor = newAdaptor(new int[] {});
		
		Assert.isFalse(adaptor.hasNext());
		Assert.isFalse(adaptor.hasNext());
		
		Assert.expect(IllegalStateException.class, new CodeBlock() {
			public void run() throws Throwable {
				adaptor.next();
			}
		});
	}
	
	public void testHasNext() {
		final int[] expected = new int[] { 1, 2, 3 };
		final Iterable4Adaptor adaptor = newAdaptor(expected);
		for (int i = 0; i < expected.length; i++) {
			assertHasNext(adaptor);
			Assert.areEqual(new Integer(expected[i]), adaptor.next());
		}
		Assert.isFalse(adaptor.hasNext());
	}
	
	public void testNext() {
		final int[] expected = new int[] { 1, 2, 3 };
		final Iterable4Adaptor adaptor = newAdaptor(expected);
		for (int i = 0; i < expected.length; i++) {
			Assert.areEqual(new Integer(expected[i]), adaptor.next());
		}
		Assert.isFalse(adaptor.hasNext());
	}
	
	private Iterable4Adaptor newAdaptor(final int[] expected) {
		return new Iterable4Adaptor(newIterable(expected));
	}

	private void assertHasNext(final Iterable4Adaptor adaptor) {
		for (int i=0; i<10; ++i) {
			Assert.isTrue(adaptor.hasNext());
		}
	}

	private Iterable4 newIterable(int[] values) {
		final Collection4 collection = new Collection4();
		collection.addAll(IntArrays4.toObjectArray(values));
		return collection;
	}
}
