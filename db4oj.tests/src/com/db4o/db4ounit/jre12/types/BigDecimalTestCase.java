/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.types;

import java.math.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class BigDecimalTestCase extends AbstractDb4oTestCase implements OptOutBigMathIssue {
	
	public static void main(String[] args) {
		new BigDecimalTestCase().runAll();
	}
	
	static String DATA = "123456789.1011121314151617181920";
	
	public static class Item {
		public BigDecimal _bigDecimal;
	}
	
	@Override
	protected void store() throws Exception {
		Item item = new Item();
		item._bigDecimal = new BigDecimal(DATA);
		store(item);
	}
	
	public void test(){
		Item item = retrieveOnlyInstance(Item.class);
		Assert.areEqual(DATA, item._bigDecimal.toString());
	}


}
