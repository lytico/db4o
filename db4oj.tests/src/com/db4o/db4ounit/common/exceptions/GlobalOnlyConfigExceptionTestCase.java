/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class GlobalOnlyConfigExceptionTestCase extends AbstractDb4oTestCase {

	public static void main(String[] args) {
		new GlobalOnlyConfigExceptionTestCase().runAll();
	}

	public void testBlockSize() {
		Assert.expect(IllegalArgumentException.class, new CodeBlock() {
			public void run() throws Throwable {
				db().configure().blockSize(-1);
			}
		});
		
		Assert.expect(IllegalArgumentException.class, new CodeBlock() {
			public void run() throws Throwable {
				db().configure().blockSize(128);
			}
		});

		Assert.expect(GlobalOnlyConfigException.class, new CodeBlock() {
			public void run() throws Throwable {
				db().configure().blockSize(12);
			}
		});
	}

	}
