/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import com.db4o.internal.*;

import db4ounit.*;

/**
 * @exclude
 */
public class IntMatcherTestCase implements TestCase {
	
	public void test(){
		Assert.isTrue(IntMatcher.ZERO.match(0));
		Assert.isFalse(IntMatcher.ZERO.match(-1));
		Assert.isFalse(IntMatcher.ZERO.match(1));
		Assert.isFalse(IntMatcher.ZERO.match(Integer.MIN_VALUE));
		Assert.isFalse(IntMatcher.ZERO.match(Integer.MAX_VALUE));
		
		Assert.isFalse(IntMatcher.POSITIVE.match(0));
		Assert.isFalse(IntMatcher.POSITIVE.match(-1));
		Assert.isTrue(IntMatcher.POSITIVE.match(1));
		Assert.isFalse(IntMatcher.POSITIVE.match(Integer.MIN_VALUE));
		Assert.isTrue(IntMatcher.POSITIVE.match(Integer.MAX_VALUE));
		
		Assert.isFalse(IntMatcher.NEGATIVE.match(0));
		Assert.isTrue(IntMatcher.NEGATIVE.match(-1));
		Assert.isFalse(IntMatcher.NEGATIVE.match(1));
		Assert.isTrue(IntMatcher.NEGATIVE.match(Integer.MIN_VALUE));
		Assert.isFalse(IntMatcher.NEGATIVE.match(Integer.MAX_VALUE));
	}

}
