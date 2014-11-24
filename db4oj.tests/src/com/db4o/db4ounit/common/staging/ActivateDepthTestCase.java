/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.staging;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @exclude
 */
public class ActivateDepthTestCase extends AbstractDb4oTestCase {
	
	public static void main(String[] args) {
		new ActivateDepthTestCase().runAll();
	}
	
	public static class Data {
		public int value ;
		public Data(int i) {
			value = i;
		}
	}
	protected void configure(Configuration config) throws Exception {
		config.activationDepth(0);
	}
	
	protected void store() throws Exception {
		store(new Data(42));
	}
	
	public void test() throws Exception {
		Data data = (Data) retrieveOnlyInstance(Data.class);
		Assert.areEqual(0, data.value);
	}

}
