/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;



import java.util.*;

import com.db4o.drs.versant.*;

import db4ounit.*;

public class VodDatabaseLifecycleTestCase implements TestCase {
	
	protected static final String DATABASE_NAME = "VodProviderTestCaseBase";
	
	public void testLifeCycle(){
		VodDatabase vod = new VodDatabase(DATABASE_NAME, new Properties());
		vod.removeDb();
		Assert.isFalse(vod.dbExists());
		vod.produceDb();
		Assert.isTrue(vod.dbExists());
		vod.removeDb();
		Assert.isFalse(vod.dbExists());
	}
	
	public void testEnhancer() {
		VodDatabase vod = new VodDatabase(DATABASE_NAME, new Properties());
		
		// TODO: Test if some known class is PersistenceCapable
		
	}

}
