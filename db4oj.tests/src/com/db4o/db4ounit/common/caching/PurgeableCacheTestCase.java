package com.db4o.db4ounit.common.caching;

import static org.easymock.EasyMock.createMock;
import static org.easymock.EasyMock.expect;
import static org.easymock.EasyMock.expectLastCall;
import static org.easymock.EasyMock.replay;
import static org.easymock.EasyMock.verify;

import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.caching.*;

import db4ounit.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class PurgeableCacheTestCase implements TestCase {
	
	final PurgeableCache4<Integer, Integer> subject = CacheFactory.newLRUCache(2);
	final Function4 producer = createMock(Function4.class);
	final Procedure4 finalizer = createMock(Procedure4.class);
	
	public void test() {
		
		expect(producer.apply(1))
			.andReturn(10);
		expect(producer.apply(2))
			.andReturn(20);
		expect(producer.apply(3))
			.andReturn(30);
		expect(producer.apply(3))
			.andReturn(30);
		expect(producer.apply(4))
			.andReturn(40);
		
		finalizer.apply(10);
		expectLastCall().asStub();
		
		replay(producer, finalizer);
		
		produce(1);
		produce(2);
		produce(3);
		
		subject.purge(3);
		produce(3);
		
		subject.purge(2);
		produce(4);
		
		IteratorAssert.sameContent(Arrays.asList(30, 40), subject);
		
		verify(producer, finalizer);
	}
	
	public void testNullIsNotCached() {
		expect(producer.apply(1))
			.andReturn(10);
		expect(producer.apply(2))
			.andReturn(20);
		expect(producer.apply(3))
			.andReturn(null);
		
		replay(producer, finalizer);
		
		produce(1);
		produce(2);
		produce(3);
		
		IteratorAssert.sameContent(Arrays.asList(10, 20), subject);
		
		verify(producer, finalizer);
	}

	private void produce(final Integer key) {
	    subject.produce(key, producer, finalizer);
    }

}
