/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.test.versant.data.*;
import com.db4o.drs.versant.*;
import com.db4o.foundation.*;
import com.db4o.io.*;

import db4ounit.*;

public class ConcurrentReplicationTestCase extends VodProviderTestCaseBase {
	
    public static void main(String[] args) {
        new ConsoleTestRunner(ConcurrentReplicationTestCase.class).run();
    }
    
    private ObjectContainer objectContainer1;
    
    private ObjectContainer objectContainer2;
    
    @Override
    public void setUp() {
        super.setUp();
        objectContainer1 = openObjectContainer("1");
        objectContainer2 = openObjectContainer("2");
    }
    
    @Override
    public void tearDown() {
    	super.tearDown();
    	objectContainer1.close();
    	objectContainer2.close();
    }
	
	public void testConcurrentReplication() throws Exception {
		
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
            	replicateConcurrently();
                return null;
            }
        });
	}
	
	private void replicateConcurrently() {
		replicate(objectContainer1);
		replicate(objectContainer2);
		storeItems();
		VodReplicationProvider vodProvider1 = openVodReplicationProvider();
		Db4oEmbeddedReplicationProvider db4oProvider1 = new Db4oEmbeddedReplicationProvider(objectContainer1);
    	final VodReplicationProvider vodProvider2 = openVodReplicationProvider();
    	final Db4oEmbeddedReplicationProvider db4oProvider2 = new Db4oEmbeddedReplicationProvider(objectContainer2);
    	replicate(db4oProvider1, vodProvider1, new ReplicationAction(){
    		@Override
    		public void beforeCommit() {
            	replicate(db4oProvider2, vodProvider2, ReplicationAction.NO_ACTIONS);
    		}
    	});
		replicate(objectContainer2);
		ObjectSet<Item> result = objectContainer2.query(Item.class);
		IteratorAssert.sameContent(new Object[] {new Item("storedIn1"), new Item("storedIn2")}, result);
	}

	private void replicate(ObjectContainer objectContainer) {
		VodReplicationProvider vodProvider = openVodReplicationProvider();
		Db4oEmbeddedReplicationProvider db4oProvider = new Db4oEmbeddedReplicationProvider(objectContainer);
		replicate(vodProvider, db4oProvider, ReplicationAction.NO_ACTIONS);
	}

	private void storeItems() {
		objectContainer1.store(new Item("storedIn1"));
		objectContainer1.commit();
		objectContainer2.store(new Item("storedIn2"));
		objectContainer2.commit();
	}
	
    private VodReplicationProvider openVodReplicationProvider() {
        VodReplicationProvider provider = new VodReplicationProvider(_vod);
        provider.listenForReplicationEvents(Holder.class, Item.class);
        return provider;
    }
	
    private ObjectContainer openObjectContainer(String name) {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configureReplication(configuration);
        return Db4oEmbedded.openFile(configuration, "!In:Memory!" + name);
    }
    
    private void configureReplication(FileConfigurationProvider config) {
        config.file().storage(new MemoryStorage());
        config.file().generateCommitTimestamps(true);
        config.file().generateUUIDs(ConfigScope.GLOBALLY);
    }

}
