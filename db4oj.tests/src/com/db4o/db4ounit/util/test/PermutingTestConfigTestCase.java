/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.util.test;

import com.db4o.db4ounit.util.*;

import db4ounit.*;

public class PermutingTestConfigTestCase implements TestCase {

	public void testPermutation() {
		Object[][] data= new Object[][] {
				new Object[] {"A","B"},
				new Object[] {"X","Y","Z"},
		};
		final PermutingTestConfig config=new PermutingTestConfig(data);
		Object[][] expected= new Object[][] {
				new Object[] {"A","X"},	
				new Object[] {"A","Y"},	
				new Object[] {"A","Z"},	
				new Object[] {"B","X"},	
				new Object[] {"B","Y"},	
				new Object[] {"B","Z"},	
		};
		for (int groupIdx = 0; groupIdx < expected.length; groupIdx++) {
			Assert.isTrue(config.moveNext());
			Object[] current={config.current(0),config.current(1)};
			ArrayAssert.areEqual(expected[groupIdx],current);
		}
		Assert.isFalse(config.moveNext());
	}
	
}
