/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant.eventlistener;

import javax.jdo.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.versant.data.*;
import com.db4o.drs.versant.*;
import com.db4o.io.*;

import db4ounit.*;

public class PreExistingObjectTestCase implements TestCase {
	
	private static final String DATABASE_NAME = "PreExisting";
	
	private static final String USER_NAME = "drs";
	
	private static final String PASSWORD = "drs";
	
	public void test(){
		VodDatabase vod = new VodDatabase(DATABASE_NAME, USER_NAME, PASSWORD);
		
		try{
			vod.removeDb();
			vod.produceDb();
			vod.addUser();
			vod.addJdoMetaDataFile(com.db4o.drs.test.versant.data.Item.class.getPackage());
			vod.createEventSchema();
			
			PersistenceManager pm = vod.persistenceManagerFactory().getPersistenceManager();
			pm.currentTransaction().begin();
			pm.makePersistent(new Item("one"));
			pm.currentTransaction().commit();
			pm.close();
			
			vod.startEventDriver();
			vod.startEventProcessor();
			
			try{
				VodReplicationProvider vodReplicationProvider = new VodReplicationProvider(vod);
				try {
					
					EmbeddedObjectContainer objectContainer = openInMemoryObjectContainer();
					ReplicationProvider replicationProvider = new Db4oEmbeddedReplicationProvider(objectContainer);
					
					Replication.begin(vodReplicationProvider, replicationProvider);
					ObjectSet replicationSet = vodReplicationProvider.objectsChangedSinceLastReplication(Item.class);
					Assert.areEqual(1, replicationSet.size());
					Item item = (Item) replicationSet.next();
					ReplicationReference ref = vodReplicationProvider.produceReference(item);
					Assert.isNotNull(ref);
					
					objectContainer.close();
				}finally {
					vodReplicationProvider.destroy();
				}
			}finally {
				vod.stopEventProcessor();
				vod.stopEventDriver();
			}
		} finally {
			vod.removeDb();
		}
	}

	private EmbeddedObjectContainer openInMemoryObjectContainer() {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().storage(new MemoryStorage());
		return Db4oEmbedded.openFile(config, "InMemory");
	}

}
