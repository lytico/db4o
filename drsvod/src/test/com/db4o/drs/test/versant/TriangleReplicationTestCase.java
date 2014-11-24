/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
package com.db4o.drs.test.versant;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.*;
import com.db4o.drs.*;
import com.db4o.drs.db4o.Db4oEmbeddedReplicationProvider;
import com.db4o.drs.test.versant.data.Item;
import com.db4o.drs.versant.VodDatabase;
import com.db4o.drs.versant.VodReplicationProvider;
import com.db4o.drs.versant.jdo.reflect.JdoReflector;
import com.db4o.foundation.Closure4;
import com.db4o.io.MemoryStorage;
import db4ounit.Assert;
import db4ounit.ConsoleTestRunner;

import javax.jdo.PersistenceManager;

import java.util.List;


public class TriangleReplicationTestCase extends VodProviderTestCaseBase {
	
    private ObjectContainer intermediateDB4O;
    
    private ObjectContainer thirdReplicationPartner;

    public static void main(String[] args) {
        new ConsoleTestRunner(TriangleReplicationTestCase.class).run();
    }

    @Override
    public void setUp() {
        super.setUp();
        this.intermediateDB4O = createDb4oInstance("IntermediateDB");
        this.thirdReplicationPartner = createDb4oInstance("ThirdReplicationPartner");
    }
    
    @Override
    public void tearDown() {
    	this.intermediateDB4O.close();
    	this.thirdReplicationPartner.close();
    }

    /**
     * In this scenario we replicate VOD with multiple db4o instances.
     *
     * Scenario: Multiple 'mobile' devices are talking to VOD
     */
    public void testReplicateWithMultiplePartners() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {

                storeSingleItemIn(thirdReplicationPartner);
                storeSingleItemIn(intermediateDB4O);

                replicate(newVODReplicationProvider(),
                        new Db4oEmbeddedReplicationProvider(intermediateDB4O));

                replicate(newVODReplicationProvider(),
                        new Db4oEmbeddedReplicationProvider(thirdReplicationPartner));

                assertItemCountInVOD(2);
                return null;
            }
        });
    }



    /**
     * First we replicate a db4o instance to a 'intermediate' instance
     * Second we replicate the  'intermediate' instance with VOD
     *
     * Scenario: A db4o gateway server for synchronisation, so that the clients don't need the VOD libraries
     */
    public void testReplicateViaIntermediateDB() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {

                storeSingleItemIn(thirdReplicationPartner);

                replicate(new Db4oEmbeddedReplicationProvider(thirdReplicationPartner),
                        new Db4oEmbeddedReplicationProvider(intermediateDB4O));

                replicate(newVODReplicationProvider(),
                        new Db4oEmbeddedReplicationProvider(intermediateDB4O));

                assertItemCountInVOD(1);


                return null;
            }
        });
    }

    /**
     * First we replicate the 'intermediate' db4o instance with VOD
     * Then we replicate the 'intermediate' db4o with a external db4o instance
     * Finally we replicate the 'intermediate' db4o instance with VOD
     *
     *
     * Scenario: A db4o gateway server for synchronisation, so that the clients don't need the VOD libraries
     */
    public void testReplicateLaterViaIntermediateDB() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {

                storeSingleItemIn(thirdReplicationPartner);

                replicate(newVODReplicationProvider(),
                        new Db4oEmbeddedReplicationProvider(intermediateDB4O));

                replicate(new Db4oEmbeddedReplicationProvider(thirdReplicationPartner),
                        new Db4oEmbeddedReplicationProvider(intermediateDB4O));

                replicate(newVODReplicationProvider(),
                        new Db4oEmbeddedReplicationProvider(intermediateDB4O));

                // Expected: One item in VOD from the third party db
                // But nothing is here?
                assertItemCountInVOD(1);

                return null;
            }
        });
    }

     /**
     * In this scenario we replicate VOD with multiple db4o instances
     * However the two db4o instance synchronize between themselves as well.
     *
     *
     * Scenario: Multiple 'mobile' devices are talking to VOD. But they have also P2P synchronisation.
     * 
     * Currently this scenario does not work yet without a ReplicationListener that is smart about
     * resolving conflicts.   
     */
    public void _testReplicateWithMultiplePartnersWithInterSynchronisation() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {

                storeSingleItemIn(thirdReplicationPartner);
                storeSingleItemIn(intermediateDB4O);

                replicate(newVODReplicationProvider(),
                        new Db4oEmbeddedReplicationProvider(intermediateDB4O));

                replicate(new Db4oEmbeddedReplicationProvider(thirdReplicationPartner),
                        new Db4oEmbeddedReplicationProvider(intermediateDB4O));

                replicate(newVODReplicationProvider(),
                        new Db4oEmbeddedReplicationProvider(thirdReplicationPartner));

                assertItemCountInVOD(2);
                return null;
            }
        });
    }

    private VodReplicationProvider newVODReplicationProvider() {
        VodReplicationProvider vodReplicationProvider = new VodReplicationProvider(new VodDatabase(_vod.persistenceManagerFactory()));
        vodReplicationProvider.ensureClassKnown(Item.class);
        return vodReplicationProvider;
    }

    private void assertItemCountInVOD(final int expectedItemCount) {
        JDOUtilities.withTransaction(_vod.persistenceManagerFactory(), new JDOTransactionClosure() {
            public void invoke(PersistenceManager pm) {
                List<Item> result = (List<Item>) pm.newQuery(Item.class).execute();
                Assert.areEqual(expectedItemCount, result.size());
            }
        });
    }

    private ObjectContainer createDb4oInstance(String name) {
        EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        storageConfig(config);
        commonCfg(config);
        return Db4oEmbedded.openFile(config, name(name));
    }

    private String name(String name) {
        return "!In:Memory:" + name + "!";
    }

    private void commonCfg(CommonConfigurationProvider config) {
        config.common().reflectWith(new JdoReflector(Thread.currentThread().getContextClassLoader()));
    }

    private void storageConfig(FileConfigurationProvider config) {
        config.file().generateCommitTimestamps(true);
        config.file().generateUUIDs(ConfigScope.GLOBALLY);
        config.file().storage(new MemoryStorage());
    }

    private void storeSingleItemIn(ObjectContainer container) {
        container.store(new Item("Item"));
        container.commit();
    }

    private void replicate(ReplicationProvider providerA,
                           ReplicationProvider providerB) {
        ReplicationSession session = Replication.begin(providerA, providerB);

        replicate(providerA, session);
        replicate(providerB, session);

        session.commit();
    }

    private void replicate(ReplicationProvider provider, ReplicationSession session) {
        for (Object object : provider.objectsChangedSinceLastReplication()) {
            session.replicate(object);
        }
    }
}
