/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;

import static com.db4o.qlin.QLinSupport.*;

import java.util.*;

import com.db4o.*;
import com.db4o.drs.versant.*;
import com.db4o.drs.versant.metadata.*;

import db4ounit.*;

public class VodCobraTestCase extends VodDatabaseTestCaseBase implements TestLifeCycle {
	
	private VodCobraFacade _cobra;
	
	public void testStore(){
		long expectedObjectLoid = 2;
		ObjectInfo objectInfo = new ObjectInfo(1, 1, expectedObjectLoid, 3, 4, 5, 0);
		long loid = _cobra.store(objectInfo);
		Assert.isGreater(0, loid);
		ObjectInfo storedObjectInfo = _cobra.objectByLoid(loid);
		Assert.areEqual(expectedObjectLoid, storedObjectInfo.objectLoid());
	}
	
	public void testQueryForExtent() {
		ObjectInfo original = new ObjectInfo(1, 1, 2, 3, 4, 5, 0);
		_cobra.store(original);
		
		Collection<ObjectInfo> result = _cobra.query(ObjectInfo.class);
		
		Assert.areEqual(1, result.size());
		ObjectInfo retrieved = result.iterator().next();
		Assert.areEqual(original, retrieved);
	}
	
	public void testCobraQLin(){
		
		long objectLoid = 42;
		
		ObjectInfo info = prototype(ObjectInfo.class);
		
		ObjectSet<ObjectInfo> events = _cobra.from(ObjectInfo.class)
			  .where(info.objectLoid())
			  .equal(objectLoid)
			  .limit(1)
			  .select();
		
		Assert.areEqual(0, events.size());
		
		_cobra.store(new ObjectInfo(1, 1, objectLoid, 3, 4, 5, 0));
		
		events = _cobra.from(ObjectInfo.class)
			  .where(info.objectLoid())
			  .equal(objectLoid)
			  .limit(1)
			  .select();
		Assert.areEqual(1, events.size());
	}
	
	public void testLockClass(){
		Assert.isTrue(_cobra.lockClass(ObjectInfo.class));
		VodCobraFacade cobra2 = VodCobra.createInstance(_vod);
		Assert.isFalse(cobra2.lockClass(ObjectInfo.class));
		_cobra.unlockClass(ObjectInfo.class);
		Assert.isTrue(cobra2.lockClass(ObjectInfo.class));
		cobra2.unlockClass(ObjectInfo.class);
	}
	
	private void ensureSchemaCreated() {
		// VodJdo.createInstance(_vod).close();
	}

	public void setUp() throws Exception {
		_cobra = VodCobra.createInstance(_vod);
		CobraReplicationSupport.initialize(_cobra);
		// ensureSchemaCreated();
	}

	public void tearDown() throws Exception {
		_cobra.close();
	}

}
