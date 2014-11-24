package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;

public class Map4TestCase implements TestCase {
	
	private final Map4 subject = new Hashtable4();
	
	public void testRemove() {
		
		for (int i=0; i<5; ++i) {
			final String key = "key" + i;
			final String value = "value" + i;
			subject.put(key, value);
			Assert.areEqual(value, subject.remove(key));
		}
	}
	
	public void testContainsKey() {
		final String key1 = "foo";
		final String key2 = "bar";
		subject.put(key1, "v");
		subject.put(key2, "v");
		Assert.isTrue(subject.containsKey(key1));
		Assert.isTrue(subject.containsKey(key2));
		Assert.isFalse(subject.containsKey(null));
		Assert.isFalse(subject.containsKey(key1.toUpperCase()));
		Assert.isFalse(subject.containsKey(key2.toUpperCase()));
	}
	
	public void testValuesIterator() {
		
		final Object[] values = new Object[5];
		for (int i=0; i<values.length; ++i) {
			values[i] = ("value" + i);
		}
		
		for (Object v : values) {
	        subject.put("key4" + v, v);
        }
		
		Iterator4Assert.sameContent(values, subject.values().iterator());
	}

}
