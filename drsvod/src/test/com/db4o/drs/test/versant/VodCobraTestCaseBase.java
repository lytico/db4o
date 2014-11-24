/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;

import com.db4o.drs.versant.*;

import db4ounit.*;


public class VodCobraTestCaseBase implements TestLifeCycle, ClassLevelFixtureTest {
	
	private static final String USER_NAME = "drs";
	
	private static final String PASSWORD = "drs";

	protected static final String DATABASE_NAME = "VodCobraTestCaseBase";
	
	protected static VodDatabase _vod;
	
	protected VodCobraFacade _cobra;
	
	public void setUp() {
		_cobra = VodCobra.createInstance(_vod);
		cleanDb();
	}

	public void tearDown() {
		_cobra.close();
	}
	
	private void cleanDb(){
		VodCobra.deleteAll(_vod);
	}
	
	public static void classSetUp() {
		if(_vod != null){
			throw new IllegalStateException();
		}
		_vod = new VodDatabase(DATABASE_NAME, USER_NAME, PASSWORD);
		_vod.removeDb();
		_vod.produceDb();
		_vod.addUser();
	}

	public static void classTearDown() {
		_vod.removeDb();
		_vod = null;
	}
}
