package com.db4o.drs.test.versant;

import javax.jdo.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.test.versant.data.*;
import com.db4o.foundation.*;
import com.db4o.io.*;

import db4ounit.*;

public class ConcurrentUpdateTestCase extends VodProviderTestCaseBase implements TestLifeCycle{
	
    private PersistenceManagerFactory persistenceFactory;

    public static void main(String[] args) {
        new ConsoleTestRunner(ConcurrentUpdateTestCase.class).run();
    }

    public void setUp() {
        super.setUp();
        storeTestObject();
    }

    private void storeTestObject() {
        this.persistenceFactory =  _vod.persistenceManagerFactory();
    }

    public void testStoreDuringReplication() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                // The item is produces before any replication. It should be there.
                storeTestItem();
                produceProvider();

                ObjectContainer container = createDb4oInstance();

                replicateFromVODToDb4o(container, new Action() {
                    public void invoke() {
                        storeTestItem();
                    }
                });
                Assert.areEqual(1, container.query(Item.class).size());

                replicateFromVODToDb4o(container, Action.NO_ACTION);

                // O boy we forgot the object which was stored during replication
                Assert.areEqual(2, container.query(Item.class).size());
                container.close();
                return null;
            }
        });
    }

    private void replicateFromVODToDb4o(ObjectContainer container, Action actionDuringReplication) {
        ReplicationProvider db4o = new Db4oEmbeddedReplicationProvider(container);
        produceProvider();

        ReplicationSession replication = Replication.begin(_provider, db4o);

        for (Object obj : _provider.objectsChangedSinceLastReplication(Item.class)) {
            replication.replicate(obj);
        }

        actionDuringReplication.invoke();

        replication.commit();


    }

    private ObjectContainer createDb4oInstance() {
        EmbeddedConfiguration cfg = Db4oEmbedded.newConfiguration();
        cfg.file().storage(new MemoryStorage());
        cfg.file().generateUUIDs(ConfigScope.GLOBALLY);
        cfg.file().generateCommitTimestamps(true);

        return Db4oEmbedded.openFile(cfg,"!InMemory!");
    }

    private void storeTestItem() {
        withTransaction(new OneArgAction<PersistenceManager>() {
            public void invoke(PersistenceManager persistenceManager) {
                persistenceManager.makePersistent(new Item());     
            }
        });
    }

    private void withTransaction(OneArgAction<PersistenceManager> toRun) {
        PersistenceManager persistence = persistenceFactory.getPersistenceManager();
        persistence.currentTransaction().begin();
        try{
            toRun.invoke(persistence);
        } catch(RuntimeException e){
            persistence.currentTransaction().rollback();
            throw e;
        }
        persistence.currentTransaction().commit();
        persistence.close();
    }
}

interface OneArgAction<TArg> {
    public void invoke(TArg arg);
}
interface Action {
    public static final Action NO_ACTION = new Action() {
        public void invoke() {
        }
    };
    public void invoke();
}
