/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.common.assorted;

import db4ounit.*;
import db4ounit.extensions.*;

public class PersistTypeTestCase extends AbstractDb4oTestCase {

	public static final class Item {
		public Class type;
		
		public Item() {			
		}
		
		public Item(Class type_) {
			type = type_;
		}
	}
	
	protected void store() throws Exception {
		store(new Item(String.class));
	}
	
	public void test() {
		Assert.areEqual(String.class, ((Item)retrieveOnlyInstance(Item.class)).type);
	}
}
