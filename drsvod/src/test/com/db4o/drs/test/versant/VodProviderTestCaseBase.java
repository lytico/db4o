/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;

import com.db4o.*;
import com.db4o.drs.*;
import com.db4o.drs.versant.*;
import com.db4o.foundation.*;

import db4ounit.*;

public abstract class VodProviderTestCaseBase  implements TestLifeCycle, ClassLevelFixtureTest  {
	
	public static abstract class ReplicationAction{
	    public static final ReplicationAction NO_ACTIONS = new ReplicationAction() {        };
	    public void beforeCommit(){}
	    public void afterCommit(){}
	}

	private static final String USER_NAME = "drs";
	
	private static final String PASSWORD = "drs";

	protected static final String DATABASE_NAME = "VodProviderTestCaseBase";
	
	protected static VodDatabase _vod;
	
	protected VodReplicationProvider _provider;
	
	// This is a direct _VodJdo connection that works around our _provider 
	// so we can see what's committed, using a second reference system.
	protected VodJdoFacade _jdo;
	
	protected VodCobraFacade _cobra;
	
	public void setUp() {
		_jdo = VodJdo.createInstance(_vod);
		_cobra = VodCobra.createInstance(_vod);
		cleanDb();
		produceProvider();
	}

	protected void produceProvider() {
		if (_provider != null) {
			_provider.destroy();
		}
		_provider = new VodReplicationProvider(_vod);
	}

	public void tearDown() {
		_cobra.close();
		_jdo.close();
		destroyProvider();
	}

	protected void destroyProvider() {
		if (_provider == null) {
			return;
		}
		_provider.destroy();
		_provider = null;
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
		_vod.addJdoMetaDataFile(com.db4o.drs.test.versant.data.Item.class.getPackage());
		_vod.createEventSchema();
		_vod.startEventDriver();
	}

	public static void classTearDown() {
		_vod.stopEventDriver();
		_vod.removeDb();
		_vod = null;
	}
	
	protected void replicate(ReplicationProvider firstProvider, ReplicationProvider secondProvider, ReplicationAction replicationActions) {
	    ReplicationSession session = Replication.begin(firstProvider, secondProvider);
	    replicate(session, firstProvider.objectsChangedSinceLastReplication());
	    replicate(session, secondProvider.objectsChangedSinceLastReplication());
	    replicationActions.beforeCommit();
	    session.commit();
	    replicationActions.afterCommit();
	    session.close();
	}

	protected void replicate(ReplicationSession session, ObjectSet objectSet) {
	    for (Object object : objectSet) {
	        session.replicate(object);
	    }
	}

	protected void withEventProcessor(Closure4<Void> closure) throws Exception {
		_vod.startEventProcessor();
		produceProvider();
		try {
			closure.run();
		} finally {
			destroyProvider();
			_vod.stopEventProcessor();
		}
	}

}
