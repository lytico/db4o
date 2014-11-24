package com.db4odoc.drs.advanced;

import com.db4o.*;
import com.db4o.config.ConfigScope;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ServerConfiguration;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.ext.ObjectInfo;

import java.io.File;


public class AdvancedReplicationExamples {
    public static final String DESKTOP_DATABASE_NAME = "desktopDatabase.db4o";
    public static final String MOBILE_DATABASE_NAME = "mobileDatabase.db4o";

    private static final String USER_NAME = "db4o";
    private static final int PORT = 4242;
    private static final String HOST = "localhost";


    public static void main(String[] args) {
        eventExample();

        replicationConflicts();
        replicationConflictTakeLatestChange();

        concurrencyLimitations();

        simpleMigration();
        migrationOnTheFly();
    }

    private static void eventExample() {
        deleteDatabases();
        storeObjectsIn(DESKTOP_DATABASE_NAME);

        ObjectContainer desktopDatabase = openDatabase(DESKTOP_DATABASE_NAME);
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);
        
        // #example: Register a listener for information about the replication process
        ReplicationSession replicationSession = Replication.begin(providerA, providerB,
                new ReplicationEventListener() {
                    public void onReplicate(ReplicationEvent replicationEvent) {
                        ObjectState stateInDesktop = replicationEvent.stateInProviderA();
                        if (stateInDesktop.isNew()) {
                            System.out.println("Object '"
                                    + stateInDesktop.getObject()
                                    + "' is new on desktop database");
                        }
                        if (stateInDesktop.wasModified()) {
                            System.out.println("Object '"
                                    + stateInDesktop.getObject()
                                    + "' was modified on desktop database");
                        }
                    }
                });
        // #end example
        replicateBidirectional(replicationSession);

        replicationSession.commit();
        closeDBs(desktopDatabase, mobileDatabase);
    }

    private static void replicationConflicts() {
        deleteDatabases();

        ObjectContainer desktopDatabase = openDatabase(DESKTOP_DATABASE_NAME);
        desktopDatabase.store(new Pilot("Max"));
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);

        replicateBidirectional(desktopDatabase, mobileDatabase);

        updateObject(desktopDatabase);
        updateObject(mobileDatabase);
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);

        // #example: Deal with conflicts
        ReplicationSession replicationSession = Replication.begin(providerA, providerB,
                new ReplicationEventListener() {
                    public void onReplicate(ReplicationEvent replicationEvent) {
                        if (replicationEvent.isConflict()) {
                            ObjectState stateOfTheDesktop = replicationEvent.stateInProviderA();
                            replicationEvent.overrideWith(stateOfTheDesktop);
                        }
                    }
                });
        // #end example

        replicateBidirectional(replicationSession);
        replicationSession.commit();

        closeDBs(desktopDatabase, mobileDatabase);
    }

    private static void replicationConflictTakeLatestChange() {
        deleteDatabases();

        ObjectContainer desktopDatabase = openDatabase(DESKTOP_DATABASE_NAME);
        desktopDatabase.store(new Pilot("Max"));
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);

        replicateBidirectional(desktopDatabase, mobileDatabase);


        updateObject(desktopDatabase);
        updateObject(mobileDatabase);
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);

        // #example: Take latest change
        ReplicationSession replicationSession = Replication.begin(providerA, providerB,
                new ReplicationEventListener() {
                    public void onReplicate(ReplicationEvent replicationEvent) {
                        if (replicationEvent.isConflict()) {
                            ObjectState stateDesktop = replicationEvent.stateInProviderA();
                            ObjectState stateMobile = replicationEvent.stateInProviderB();

                            if (stateDesktop.modificationDate() >= stateMobile.modificationDate()) {
                                replicationEvent.overrideWith(stateDesktop);
                            } else {
                                replicationEvent.overrideWith(stateMobile);
                            }
                        }
                    }
                });
        // #end example

        replicateBidirectional(replicationSession);
        replicationSession.commit();

        closeDBs(desktopDatabase, mobileDatabase);
    }


    private static void concurrencyLimitations() {
        deleteDatabases();

        // #example: Lost replication
        ObjectServer serverDatabase = openDatabaseServer(DESKTOP_DATABASE_NAME);
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);

        {

            ObjectContainer serverDbConnection =
                    Db4oClientServer.openClient(HOST, PORT, USER_NAME, USER_NAME);
            serverDbConnection.store(new Pilot("Pilot 1"));
            serverDbConnection.commit();

            // The replication starts here
            ObjectContainer connectionForReplication
                    = Db4oClientServer.openClient(HOST, PORT, USER_NAME, USER_NAME);
            
            Db4oEmbeddedReplicationProvider providerA = new Db4oClientServerReplicationProvider(connectionForReplication);
            Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);
            
            ReplicationSession replicationSession = Replication.begin(providerA, providerB);
            ObjectSet changesOnDesktop
                    = replicationSession.providerA().objectsChangedSinceLastReplication();

            // during the replication other clients store data on the server
            serverDbConnection.store(new Pilot("Pilot 2"));
            serverDbConnection.commit();

            for (Object changedObjectOnDesktop : changesOnDesktop) {
                replicationSession.replicate(changedObjectOnDesktop);
            }

            replicationSession.commit();

            serverDbConnection.store(new Pilot("Pilot 3"));
            serverDbConnection.commit();

        }

        // Pilot 2 is not replicated
        printPilots(mobileDatabase);


        {
            ObjectContainer connectionForReplication
                    = Db4oClientServer.openClient(HOST, PORT, USER_NAME, USER_NAME);
            
            Db4oEmbeddedReplicationProvider providerA = new Db4oClientServerReplicationProvider(connectionForReplication);
            Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);
            
            ReplicationSession replicationSession= Replication.begin(providerA, providerB);
            ObjectSet changesOnDesktop
                    = replicationSession.providerA().objectsChangedSinceLastReplication();
            for (Object changedOnDesktop : changesOnDesktop) {
                replicationSession.replicate(changedOnDesktop);
            }
            replicationSession.commit();
        }

        // Pilot 2 is still not replicated
        printPilots(mobileDatabase);
        // #end example

        serverDatabase.close();
        mobileDatabase.close();
    }


    private static void simpleMigration() {
        deleteDatabases();

        ObjectContainer desktopDatabaseWithoutUUID = Db4oEmbedded.openFile(DESKTOP_DATABASE_NAME);
        desktopDatabaseWithoutUUID.store(new Pilot("Max"));
        desktopDatabaseWithoutUUID.store(new Pilot("Joe"));
        desktopDatabaseWithoutUUID.commit();
        desktopDatabaseWithoutUUID.close();

        ObjectContainer desktopDatabase = openDatabase(DESKTOP_DATABASE_NAME);
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);

        // #example: Updating all objects ensures that it has a UUID and timestamp
        ObjectSet<Object> allObjects = desktopDatabase.query(Object.class);
        for (Object objectToUpdate : allObjects) {
            desktopDatabase.store(objectToUpdate);
        }
        desktopDatabase.commit();
        // #end example

        replicateBidirectional(desktopDatabase, mobileDatabase);

        printPilots(mobileDatabase);

        closeDBs(desktopDatabase, mobileDatabase);

    }


    private static void migrationOnTheFly() {
        deleteDatabases();

        ObjectContainer desktopDatabaseWithoutUUID = Db4oEmbedded.openFile(DESKTOP_DATABASE_NAME);
        desktopDatabaseWithoutUUID.store(new Car(new Pilot("Max"), "Max's Car"));
        desktopDatabaseWithoutUUID.store(new Car(new Pilot("Joe"), "Joe's Car"));
        desktopDatabaseWithoutUUID.commit();
        desktopDatabaseWithoutUUID.close();

        ObjectContainer desktopDatabase = openDatabase(DESKTOP_DATABASE_NAME);
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);

        // #example: Migrate on the fly
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oClientServerReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);

        ReplicationSession replicationSession = Replication.begin(providerA, providerB);
        ObjectSet initialReplication = desktopDatabase.query(Car.class);

        for (Object changedObjectOnDesktop : initialReplication) {
            ObjectInfo infoAboutObject = desktopDatabase.ext().getObjectInfo(changedObjectOnDesktop);
            if (null == infoAboutObject.getUUID()) {
                desktopDatabase.ext().store(changedObjectOnDesktop, 2);
            }
            replicationSession.replicate(changedObjectOnDesktop);
        }
        replicationSession.commit();
        // #end example

        printCars(mobileDatabase);

        closeDBs(desktopDatabase, mobileDatabase);

    }


    private static void updateObject(ObjectContainer desktopDatabase) {
        Pilot pilotOnDesktop = desktopDatabase.query(Pilot.class).get(0);
        pilotOnDesktop.setPoints(200);
        desktopDatabase.store(pilotOnDesktop);
        desktopDatabase.commit();
    }


    private static void replicateBidirectional(ObjectContainer desktopDatabase, ObjectContainer mobileDatabase) {
        Db4oEmbeddedReplicationProvider providerA = new Db4oClientServerReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);
        ReplicationSession replicationSession = Replication.begin(providerA, providerB);
        replicateBidirectional(replicationSession);
        replicationSession.commit();
    }


    private static void replicateBidirectional(ReplicationSession replication) {
        ObjectSet changesOnDesktop = replication.providerA().objectsChangedSinceLastReplication();
        ObjectSet changesOnMobile = replication.providerB().objectsChangedSinceLastReplication();

        for (Object changedObjectOnDesktop : changesOnDesktop) {
            replication.replicate(changedObjectOnDesktop);
        }

        for (Object changedObjectOnMobile : changesOnMobile) {
            replication.replicate(changedObjectOnMobile);
        }
    }

    private static ObjectContainer openDatabase(String fileName) {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().generateUUIDs(ConfigScope.GLOBALLY);
        configuration.file().generateCommitTimestamps(true);
        return Db4oEmbedded.openFile(configuration, fileName);
    }

    private static ObjectServer openDatabaseServer(String fileName) {
        ServerConfiguration configuration = Db4oClientServer.newServerConfiguration();
        configuration.file().generateUUIDs(ConfigScope.GLOBALLY);
        configuration.file().generateCommitTimestamps(true);
        ObjectServer srv = Db4oClientServer.openServer(configuration, fileName, PORT);
        srv.grantAccess(USER_NAME, USER_NAME);
        return srv;
    }

    private static void storeObjectsIn(String databaseFile) {
        ObjectContainer db = openDatabase(databaseFile);
        Pilot john = new Pilot("John", 100);
        Car johnsCar = new Car(john, "John's Car");
        db.store(johnsCar);
        Pilot max = new Pilot("Max", 200);
        Car maxsCar = new Car(max, "Max's Car");
        db.store(maxsCar);
        db.commit();
        db.close();
    }

    private static void printCars(ObjectContainer database) {
        ObjectSet<Car> cars = database.query(Car.class);
        for (Car car : cars) {
            System.out.println(car);
        }
    }

    private static void printPilots(ObjectContainer database) {
        ObjectSet<Pilot> pilots = database.query(Pilot.class);
        for (Pilot pilot : pilots) {
            System.out.println(pilot);
        }
    }


    private static void closeDBs(ObjectContainer... databases) {
        for (ObjectContainer db : databases) {
            db.close();
        }
    }


    private static void deleteDatabases() {
        new File(DESKTOP_DATABASE_NAME).delete();
        new File(MOBILE_DATABASE_NAME).delete();
    }

}
