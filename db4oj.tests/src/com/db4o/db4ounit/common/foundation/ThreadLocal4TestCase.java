package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;

public class ThreadLocal4TestCase implements TestCase {
	
	public void testSet() {
		final Object value = new Object();
		final ThreadLocal4<Object> local = new ThreadLocal4<Object>();
		local.set(value);
		Assert.areSame(value, local.get());
		
		local.set(null);
		Assert.areSame(null, local.get());
	}

}
