/* Copyright (C) 2011 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.types;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class StoreTopLevelPrimitiveTestCase extends AbstractDb4oTestCase {
	
	public static void main(String[] args) {
		new StoreTopLevelPrimitiveTestCase().runAll();
	}
	
	public void test() {
		boolean exceptionHappened = false;
		try {
			store(new Integer(42));
		}
		catch(ObjectNotStorableException onsex){
			exceptionHappened = true;
			StringAssert.contains("Value types", onsex.getMessage());
		}
		
		Assert.isTrue(exceptionHappened);
	}
}
