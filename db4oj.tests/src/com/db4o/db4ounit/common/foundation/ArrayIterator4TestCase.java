/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;


public class ArrayIterator4TestCase implements TestCase {
	
	public void testEmptyArray() {
		assertExhausted(new ArrayIterator4(new Object[0])); 
	}
	
	public void testArray() {
		ArrayIterator4 i = new ArrayIterator4(new Object[] { "foo", "bar" });
		Assert.isTrue(i.moveNext());
		Assert.areEqual("foo", i.current());
		
		Assert.isTrue(i.moveNext());
		Assert.areEqual("bar", i.current());
		
		assertExhausted(i);
	}
	
	private void assertExhausted(final ArrayIterator4 i) {
		Assert.isFalse(i.moveNext());		
		Assert.expect(ArrayIndexOutOfBoundsException.class, new CodeBlock(){
			public void run() throws Throwable {
				System.out.println(i.current());
			}
		});
	}

}
