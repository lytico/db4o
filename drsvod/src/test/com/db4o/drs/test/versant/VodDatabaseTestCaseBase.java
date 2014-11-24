/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;

import com.db4o.drs.versant.*;

import db4ounit.*;

public abstract class VodDatabaseTestCaseBase implements TestCase, ClassLevelFixtureTest {
	
	private static final String USER_NAME = "drs";
	
	private static final String PASSWORD = "drs";
	
	private static final String DATABASE_NAME = "VodDatabaseTestCaseBase";
	
	public static void classSetUp() {
		_vod = new VodDatabase(DATABASE_NAME, USER_NAME, PASSWORD);
		_vod.produceDb();
		_vod.addUser();
	}

	public static void classTearDown() {
		_vod.removeDb();
	}
	
	protected static VodDatabase _vod;

}
