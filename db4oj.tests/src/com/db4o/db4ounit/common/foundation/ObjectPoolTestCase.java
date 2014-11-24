package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;

public class ObjectPoolTestCase implements TestCase {
	
	public void test() {
		
		final Object o1 = new Object();
		final Object o2 = new Object();
		final Object o3 = new Object();
		
		final ObjectPool<Object> pool = new SimpleObjectPool<Object>(o1, o2, o3);
		Assert.areSame(o1, pool.borrowObject());
		Assert.areSame(o2, pool.borrowObject());
		Assert.areSame(o3, pool.borrowObject());
		
		Assert.expect(IllegalStateException.class, new CodeBlock() {
			public void run() throws Throwable {
				pool.borrowObject();
            }
		});
		
		pool.returnObject(o2);
		Assert.areSame(o2, pool.borrowObject());
	}
}
