/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions;

import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class BackupCSExceptionTestCase extends Db4oClientServerTestCase {
	public static void main(String[] args) {
		new BackupCSExceptionTestCase().runAll();
	}

	public void test() {
		Assert.expect(NotSupportedException.class, new CodeBlock() {
			public void run() throws Throwable {
				db().backup("");
			}
		});

	}
}
