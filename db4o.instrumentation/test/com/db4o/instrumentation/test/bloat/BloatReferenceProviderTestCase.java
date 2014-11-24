/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.test.bloat;

import com.db4o.instrumentation.bloat.*;

import db4ounit.*;

public class BloatReferenceProviderTestCase implements TestCase {
	
	public void testCachedTypes() {
		final BloatReferenceProvider provider = new BloatReferenceProvider();
		Assert.areSame(provider.forType(Integer.TYPE), provider.forType(Integer.TYPE));
	}

}
