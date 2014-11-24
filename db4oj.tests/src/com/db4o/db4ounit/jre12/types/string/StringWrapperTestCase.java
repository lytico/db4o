/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.types.string;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class StringWrapperTestCase extends AbstractDb4oTestCase {
	public static void main(String[] args) {
		new StringWrapperTestCase().runSolo();
	}

	public void test() throws Exception {
		store(new StringItem("hello"));
		reopen();
		StringItem item = (StringItem) retrieveOnlyInstance(StringItem.class);
		Assert.areEqual("hello", item.str);		
	}
	
	/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class StringItem {
		public Comparable str;
		public StringItem(Comparable c) {
			str = c;
		}
	}
}
