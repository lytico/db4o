package com.db4odoc.drs.vod;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.ConfigScope;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.drs.Replication;
import com.db4o.drs.ReplicationProvider;
import com.db4o.drs.ReplicationSession;
import com.db4o.drs.db4o.Db4oEmbeddedReplicationProvider;
import com.db4o.drs.versant.VodDatabase;
import com.db4o.drs.versant.VodReplicationProvider;
import com.db4o.drs.versant.jdo.reflect.JdoReflector;

import javax.jdo.PersistenceManager;
import javax.jdo.PersistenceManagerFactory;
import java.io.File;
import java.util.List;

import static com.db4odoc.drs.vod.JDOUtilities.inTransaction;


public class SimpleReplication {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        fromDB4OtoVOD();
        fromVODtoDB4O();
        biDirectional();

    }

    private static void fromDB4OtoVOD() {
        cleanUp();
        ObjectContainer container = Db4oEmbedded.openFile(newConfiguration(), DATABASE_FILE);
        PersistenceManagerFactory factory = JDOUtilities.createPersistenceFactory();
        try {
            storeAFewObjects(container);

            // #example: Open the db4o replication provider
            ReplicationProvider mobileDatabase
                    = new Db4oEmbeddedReplicationProvider(container);
            // #end example

            // #example: Open the VOD replication provider
            VodDatabase vodDatabase = new VodDatabase(factory);
            vodDatabase.configureEventProcessor("localhost",4088);
            VodReplicationProvider centralDatabase
                    = new VodReplicationProvider(vodDatabase);
            centralDatabase.listenForReplicationEvents(Car.class, Pilot.class);
            // #end example

            // #example: Start replication
            ReplicationSession replicationSession =
                    Replication.begin(mobileDatabase, centralDatabase);

            ObjectSet changesOnMobileDB = mobileDatabase.objectsChangedSinceLastReplication();
            for (Object changedObject : changesOnMobileDB) {
                replicationSession.replicate(changedObject);
            }
            replicationSession.commit();
            // #end example

            listAll(container);
            listAll(JDOUtilities.createPersistenceFactory());
        } finally {
            container.close();
        }
    }

    private static void fromVODtoDB4O() {
        cleanUp();
        ObjectContainer container = Db4oEmbedded.openFile(newConfiguration(), DATABASE_FILE);
        try {
            PersistenceManagerFactory factory = JDOUtilities.createPersistenceFactory();
            storeAFewObjects(factory);


            ReplicationProvider mobileDatabase
                    = new Db4oEmbeddedReplicationProvider(container);

            VodReplicationProvider centralDatabase
                    = new VodReplicationProvider(new VodDatabase(factory));
            centralDatabase.listenForReplicationEvents(Car.class, Pilot.class);

            // #example: From VOD to db4o
            ReplicationSession replicationSession =
                    Replication.begin(mobileDatabase, centralDatabase);

            ObjectSet changesOnVOD = centralDatabase.objectsChangedSinceLastReplication();
            for (Object changedObject : changesOnVOD) {
                replicationSession.replicate(changedObject);
            }
            replicationSession.commit();
            // #end example

            listAll(container);
            listAll(factory);
        } finally {
            container.close();
        }
    }

    private static void biDirectional() {
        cleanUp();
        ObjectContainer container = Db4oEmbedded.openFile(newConfiguration(), DATABASE_FILE);
        try {
            PersistenceManagerFactory persistenceFactory = JDOUtilities.createPersistenceFactory();
            storeAFewObjects(persistenceFactory);
            storeAFewObjects(container);


            ReplicationProvider mobileDatabase
                    = new Db4oEmbeddedReplicationProvider(container);

            VodReplicationProvider centralDatabase
                    = new VodReplicationProvider(new VodDatabase(persistenceFactory));
            centralDatabase.listenForReplicationEvents(Car.class, Pilot.class);

            // #example: Bidirectional replication
            ReplicationSession replicationSession =
                    Replication.begin(mobileDatabase, centralDatabase);

            ObjectSet changesOnDB4O = mobileDatabase.objectsChangedSinceLastReplication();
            for (Object changedObject : changesOnDB4O) {
                replicationSession.replicate(changedObject);
            }

            ObjectSet changesOnVOD = centralDatabase.objectsChangedSinceLastReplication();
            for (Object changedObject : changesOnVOD) {
                replicationSession.replicate(changedObject);
            }
            replicationSession.commit();
            // #end example

            listAll(container);
            listAll(persistenceFactory);
        } finally {
            container.close();
        }
    }

    private static void listAll(ObjectContainer rootContainer) {
        ObjectContainer container = rootContainer.ext().openSession();
        try {
            System.out.println("List of object in db4o:");
            listAll(container.query(Pilot.class));
            listAll(container.query(Car.class));
        } finally {
            container.close();
        }
    }

    private static void listAll(PersistenceManagerFactory factory) {
            System.out.println("List of object in VOD:");
        inTransaction(factory, new JDOTransaction() {
            public void invoke(PersistenceManager manager) {
                listAll((List<?>) manager.newQuery(Pilot.class).execute());
                listAll((List<?>) manager.newQuery(Car.class).execute());
            }
        });
    }

    private static void listAll(Iterable<?> objectSet) {
        for (Object obj : objectSet) {
            System.out.println(obj);
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
        PersistenceManagerFactory factory = JDOUtilities.createPersistenceFactory();
        inTransaction(factory, new JDOTransaction() {
            public void invoke(PersistenceManager manager) {
                manager.newQuery(Car.class).deletePersistentAll();
                manager.newQuery(Pilot.class).deletePersistentAll();
            }
        });
    }

    private static <T> T cast(Object execute) {
        return (T) execute;
    }


    private static void storeAFewObjects(ObjectContainer rootContainer) {
        ObjectContainer container = rootContainer.ext().openSession();
        try {
            container.store(new Car(new Pilot("John"), "VM Golf"));
            container.store(new Car(new Pilot("Joanna"), "Toyota"));
        } finally {
            container.close();
        }
    }

    private static void storeAFewObjects(PersistenceManagerFactory factory) {
        inTransaction(factory, new JDOTransaction() {
            public void invoke(PersistenceManager manager) {
                manager.makePersistent(new Car(new Pilot("John"), "VM Golf"));
                manager.makePersistent(new Car(new Pilot("Joanna"), "Toyota"));
            }
        });
    }

    private static EmbeddedConfiguration newConfiguration() {
        // #example: Enable UUIDs and commit timestamps use the JDO reflector
        EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        config.file().generateUUIDs(ConfigScope.GLOBALLY);
        config.file().generateCommitTimestamps(true);
        config.common().reflectWith(
                new JdoReflector(Thread.currentThread().getContextClassLoader()));
        // #end example
        return config;
    }
}
