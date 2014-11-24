/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.io;

import com.db4o.db4ounit.common.api.*;
import com.db4o.io.*;

import db4ounit.fixtures.*;

public class StorageTestUnitBase extends TestWithTempFile {

	protected Bin _bin;

	public StorageTestUnitBase() {
		super();
	}

	@Override
	public void setUp() throws Exception {
    	super.setUp();
    	open(false);
    }

	protected void open(final boolean readOnly) {
		if (null != _bin) {
			throw new IllegalStateException();
		}
	    _bin = storage().open(new BinConfiguration(tempFile(), false, 0, readOnly));
    }

	@Override
	public void tearDown() throws Exception {
    	close();
    	super.tearDown();
    }

	protected void close() {
	    if (null != _bin) {
	    	_bin.sync();
    		_bin.close();
    		_bin = null;
    	}
    }

	private Storage storage() {
    	return SubjectFixtureProvider.value();
    }

}