/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.cs;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class PrefetchObjectCountZeroTestCase extends AbstractDb4oTestCase implements OptOutAllButNetworkingCS {

	public static class Item {
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.clientServer().prefetchObjectCount(0);
	}

	@Override
	protected void store() throws Exception {
		store(new Item());
	}

	public void testZeroPrefetchObjectCount() {
		Assert.isNotNull(retrieveOnlyInstance(Item.class));
	}
}
