/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.io;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.ext.*;
import com.db4o.io.*;

import db4ounit.*;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public abstract class DiskFullTestCaseBase extends Db4oTestWithTempFile {
	
	protected abstract ThrowCondition createThrowCondition(Object conditionConfig);

	protected abstract void configureForFailure(ThrowCondition condition);

	private ObjectContainer _db;
	private ThrowCondition _throwCondition;

	public DiskFullTestCaseBase() {
		super();
	}

	public void tearDown() throws Exception {
		if(_db != null) {
			_db.close();
			_db = null;
		}
		super.tearDown();
	}

	protected void storeOneAndFail(Object conditionConfig, boolean doCache) {
		openDatabase(conditionConfig, false, doCache);
		_db.store(new Item(42));
		_db.commit();
		triggerDiskFullAndClose();
	}

	protected void storeNAndFail(Object conditionConfig, int numObjects, int commitInterval, boolean doCache) {
		openDatabase(conditionConfig, false, doCache);
		for(int objIdx = 0; objIdx < numObjects; objIdx++) {
			_db.store(new Item(objIdx));
			if(objIdx % commitInterval == commitInterval - 1) {
				_db.commit();
			}
		}
		triggerDiskFullAndClose();
	}

	protected void assertItemsStored(int numItems, Object conditionConfig, boolean readOnly, boolean withCache) {
		Assert.isNull(_db);
		openDatabase(conditionConfig, readOnly, false);
		int itemCount = _db.query(Item.class).size();
		if(withCache){
			Assert.isTrue(itemCount == numItems || itemCount == numItems + 1);
			
		}else{
			Assert.areEqual(numItems, itemCount);
		}
		closeDb();
	}


	protected void triggerDiskFullAndClose() {
		configureForFailure(_throwCondition);
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				_db.store(new Item(42));
				_db.commit();
			}
		});
		_db = null;
	}

	public void openDatabase(Object conditionConfig, boolean readOnly, boolean doCache) {
		EmbeddedConfiguration config = newConfiguration();
		_throwCondition = createThrowCondition(conditionConfig);
		config.file().freespace().discardSmallerThan(Integer.MAX_VALUE);
		config.file().readOnly(readOnly);
		configureIoAdapter(config, _throwCondition, doCache);
		_db = Db4oEmbedded.openFile(config, tempFile());
	}

	private void configureIoAdapter(EmbeddedConfiguration config, ThrowCondition throwCondition, boolean doCache) {
		Storage storage = new FileStorage();
		storage = new ThrowingStorage(storage, throwCondition);
		if(doCache) {
			storage = new CachingStorage(storage, 256, 2);
		}
		config.file().storage(storage);
	}

	protected void closeDb() {
		try {
			_db.close();
		}
		finally {
			_db = null;
		}
	}

}
