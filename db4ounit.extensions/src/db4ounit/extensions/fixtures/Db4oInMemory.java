/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.fixtures;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.io.*;

import db4ounit.extensions.*;

public class Db4oInMemory extends AbstractSoloDb4oFixture {
    
	private static final String DB_URI = "test_db";

	public Db4oInMemory() {
		super();
	}
	
	public Db4oInMemory(FixtureConfiguration fc) {
		this();
		fixtureConfiguration(fc);
	}
	
	@Override
	public boolean accept(Class clazz) {
		if (!super.accept(clazz)) {
			return false;
		}
		if (OptOutInMemory.class.isAssignableFrom(clazz)) {
			return false;
		}
		return true;
	}

	private final PagingMemoryStorage _storage = new PagingMemoryStorage(63);
	
	protected ObjectContainer createDatabase(Configuration config) {
		return Db4o.openFile(config, DB_URI);
	}

	protected Configuration newConfiguration() {
		Configuration config = super.newConfiguration();
		config.storage(_storage);
		return config;
	}

    protected void doClean() {
    	try {
			_storage.delete(DB_URI);
		} 
    	catch (IOException exc) {
			exc.printStackTrace();
		}
    }

	public String label() {
		return buildLabel("IN-MEMORY");
	}

	public void defragment() throws Exception {
		defragment(DB_URI);
	}
	
}
