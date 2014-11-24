package com.db4o.drs.test.versant;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.ConfigScope;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.drs.Replication;
import com.db4o.drs.ReplicationProvider;
import com.db4o.drs.ReplicationSession;
import com.db4o.drs.db4o.Db4oEmbeddedReplicationProvider;
import com.db4o.drs.test.versant.data.Holder;
import com.db4o.drs.test.versant.data.Item;
import com.db4o.drs.versant.VodDatabase;
import com.db4o.drs.versant.VodReplicationProvider;
import com.db4o.drs.versant.jdo.reflect.JdoReflector;
import com.db4o.foundation.Closure4;
import com.db4o.foundation.Function4;
import com.db4o.io.MemoryStorage;
import db4ounit.Assert;
import db4ounit.ConsoleTestRunner;

import javax.jdo.PersistenceManager;
import javax.jdo.PersistenceManagerFactory;

public class ExplicitListenTestCase extends VodProviderTestCaseBase{
	
    private ObjectContainer rootContainer;

    public static void main(String[] args) {
        new ConsoleTestRunner(ExplicitListenTestCase.class).run();
    }
    @Override
    public void setUp() {
        super.setUp();
        this.rootContainer = newContainer();
    }

    @Override
    public void tearDown() {
        super.tearDown();
        rootContainer.close();
    }

    /**
     * If objects are stored before replicating them the first time,
     * listenForReplicationEvents has to be called.
     */
    public void testVodToDdb4oReplication() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
            	listenForReplicationEvents(Item.class);
                storeTestItemsIn(_vod);
                replicateFromVodToDb4o();
                assertObjectsInDb4o(Item.class,1);
                return null;
            }

        });
    }
    
	private void listenForReplicationEvents(Class clazz) {
		VodReplicationProvider vodReplicationProvider = new VodReplicationProvider(_vod);
		vodReplicationProvider.listenForReplicationEvents(clazz);
		vodReplicationProvider.destroy();
		
	}

    /**
     * The replication works if the class is explicitly specified
     */
    public void testVodToDb4oWithExplicitClassReferences() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                storeTestItemsIn(_vod);
                replicateFromVodToDb4oWithExplicitClasses();
                assertObjectsInDb4o(Item.class,1);
                return null;
            }
        });
    }
    /**
     * If you add new classes you have to call listenForReplicationEvents 
     */
    public void testVodToDdb4oWithNewClasses() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                // First we introduce a previous replication from db4o to VOD
                storeTestItemsIn(rootContainer);
                replicateFromDb4oToVOD();
                
                listenForReplicationEvents(Holder.class);

                // New we introduce the new holder instances
                storeTestHolderIn(_vod);
                replicateFromVodToDb4o();
                // the items are replicated
                assertObjectsInDb4o(Item.class,2);
                // but no the new holder-instances
                assertObjectsInDb4o(Holder.class,1);
                return null;
            }

        });
    }


    /**
     * The replication works if the class was 'touched' by a db4o to VOD replication
     */
    public void testVodToDdb4oReplicationWithPreviousReplication() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                // First we introduce a previous replication from db4o to VOD
                storeTestItemsIn(rootContainer);
                replicateFromDb4oToVOD();

                storeTestItemsIn(_vod);
                replicateFromVodToDb4o();
                assertObjectsInDb4o(Item.class,2);
                return null;
            }

        });
    }

    private void assertObjectsInDb4o(Class<?> type,int count) {
        ObjectContainer session = rootContainer.ext().openSession();
        try{
            ObjectSet<?> items = session.query(type);
            Assert.areEqual(count,items.size());
        } finally {
            session.close();
        }
    }

    private void replicateFromDb4oToVOD() {
        replicate(new Function4<ReplicationSource, ReplicationProvider>() {
            public ReplicationProvider apply(ReplicationSource arg) {
                return arg.db4oReplicationPeer;
            }
        });
    }

    private void replicateFromVodToDb4o() {
        replicate(new Function4<ReplicationSource, ReplicationProvider>() {
            public ReplicationProvider apply(ReplicationSource arg) {
                return arg.vodReplicationPeer;
            }
        });
    }
    private void replicateFromVodToDb4oWithExplicitClasses() {
        replicate(new Function4<ReplicationSource, ReplicationProvider>() {
            public ReplicationProvider apply(ReplicationSource arg) {
                return arg.vodReplicationPeer;
            }
        },new Function4<ReplicationProvider, Iterable>() {
            public Iterable apply(ReplicationProvider arg) {
                return arg.objectsChangedSinceLastReplication(Item.class);
            }
        });
    }

    private void replicate(Function4<ReplicationSource,ReplicationProvider> selector) {
        replicate(selector,new Function4<ReplicationProvider, Iterable>() {
            public Iterable apply(ReplicationProvider arg) {
                return arg.objectsChangedSinceLastReplication();
            }
        });
    }

    private void replicate(Function4<ReplicationSource,ReplicationProvider> changeSource,
                        Function4<ReplicationProvider,Iterable> changeSet) {

        ReplicationSource source = new ReplicationSource();

        ReplicationSession replicationSession
                = Replication.begin(source.db4oReplicationPeer, source.vodReplicationPeer);

        Iterable changedObjectsOnVOD
                = changeSet.apply(changeSource.apply(source));
        for (Object changedObject : changedObjectsOnVOD) {
            replicationSession.replicate(changedObject);
        }

        replicationSession.commit();
        
        source.close();
    }


    private void storeTestItemsIn(VodDatabase db) {
        withTransaction(db.persistenceManagerFactory(), new JDOTransactionClosure() {
            public void invoke(PersistenceManager transaction) {
                transaction.makePersistent(new Item("Data"));
            }
        });
    }

    private void storeTestHolderIn(VodDatabase db) {
        withTransaction(db.persistenceManagerFactory(), new JDOTransactionClosure() {
            public void invoke(PersistenceManager transaction) {
                transaction.makePersistent(new Holder(new Item("Data")));
            }
        });
    }
    private void storeTestItemsIn(ObjectContainer rootContainer) {
        withTransaction(rootContainer, new DB4OTransactionClosure() {
            public void invoke(ObjectContainer transaction) {
                transaction.store(new Item("Data"));
            }
        });
    }

    private static ObjectContainer newContainer() {
        EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        config.file().generateUUIDs(ConfigScope.GLOBALLY);
        config.file().generateCommitTimestamps(true);
        config.common().reflectWith(
                new JdoReflector(Thread.currentThread().getContextClassLoader()));
        config.file().storage(new MemoryStorage());
        return Db4oEmbedded.openFile(config,"In:Memory");
    }

    private static void withTransaction(PersistenceManagerFactory factory, JDOTransactionClosure transactionalOperation) {
        PersistenceManager session = factory.getPersistenceManager();
        try{
            session.currentTransaction().begin();
            transactionalOperation.invoke(session);
            session.currentTransaction().commit();
        } catch (Exception e){
            session.currentTransaction().rollback();
            reThrow(e);
        } finally {
            session.close();
        }
    }
    private static void withTransaction(ObjectContainer rootContainer, DB4OTransactionClosure transactionalOperation) {
        ObjectContainer session = rootContainer.ext().openSession();
        try{
            transactionalOperation.invoke(session);
        } catch (Exception e){
            session.rollback();
            reThrow(e);
        } finally {
            session.close();
        }
    }

    private static void reThrow(Exception e) {
        ExplicitListenTestCase.<RuntimeException>throwsUnchecked(e);
    }
    private static <T extends Exception> void throwsUnchecked(Exception toThrow) throws T{
        // Since the type is erased, this cast actually does nothing!!!
        // we can throw any exception
        throw (T) toThrow;
    }


    class ReplicationSource{
        private final Db4oEmbeddedReplicationProvider db4oReplicationPeer;
        private final VodReplicationProvider vodReplicationPeer;

        ReplicationSource(){
            this.db4oReplicationPeer
                    = new Db4oEmbeddedReplicationProvider(rootContainer);
            this.vodReplicationPeer
                    = new VodReplicationProvider(_vod);
        }
        
        public void close(){
        	db4oReplicationPeer.destroy();
        	vodReplicationPeer.destroy();
        }
    }
}
