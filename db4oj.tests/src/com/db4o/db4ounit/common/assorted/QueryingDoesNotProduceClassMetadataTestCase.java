/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import db4ounit.*;
import db4ounit.extensions.*;

public class QueryingDoesNotProduceClassMetadataTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
		
	}
	
	public void test(){
		db().query(Item.class);
		Assert.isNull(container().classMetadataForName(Item.class.getName()));
	}

}
