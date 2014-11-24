/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.updatedepth;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.internal.*;

import db4ounit.*;

public class NegativeUpdateDepthTestCase implements TestCase {

	public static class Item {
	}
	
	public void testNegativeUpdateDepthIsIllegal() {
		final CommonConfiguration config = Db4oEmbedded.newConfiguration().common();
		Assert.expect(IllegalArgumentException.class, new CodeBlock() {
			public void run() throws Throwable {
				config.objectClass(Item.class).updateDepth(Const4.UNSPECIFIED);
			}
		});
	}
	
}
