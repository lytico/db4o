/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.config;

import com.db4o.*;
import com.db4o.config.*;

import db4ounit.*;

public class ConfigurationOfObjectClassNotSupportedTestCase implements TestCase {
	
	public void test(){
		final EmbeddedConfiguration embeddedConfiguration = Db4oEmbedded.newConfiguration();
		Assert.expect(IllegalArgumentException.class, new CodeBlock() {
			public void run() throws Throwable {
				embeddedConfiguration.common().objectClass(Object.class);
			}
		});
		
		
	}

}
