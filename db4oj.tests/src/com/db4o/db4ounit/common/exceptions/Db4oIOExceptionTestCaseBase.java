/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions;

import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.io.*;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class Db4oIOExceptionTestCaseBase
	extends AbstractDb4oTestCase
	implements OptOutMultiSession, OptOutTA {
	
	private ExceptionSimulatingStorage _storage;
	
	protected void configure(Configuration config) {
		config.lockDatabaseFile(false);
		_storage = new ExceptionSimulatingStorage(new FileStorage(), new ExceptionFactory() {
			public void throwException() {
				throw new Db4oIOException();
			}

			public void throwOnClose() {
			}
		});
		config.storage(_storage);
	}
	
	protected void db4oSetupBeforeStore() throws Exception {
		triggerException(false);
	}

	protected void db4oTearDownBeforeClean() throws Exception {
		triggerException(false);
	}
	
	protected void triggerException(boolean value) {
		_storage.triggerException(value);
	}
}
