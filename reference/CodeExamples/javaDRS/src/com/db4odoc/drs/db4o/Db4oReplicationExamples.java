package com.db4odoc.drs.db4o;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.ConfigScope;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.query.Predicate;

import java.io.File;

public class Db4oReplicationExamples {
    public static final String DESKTOP_DATABASE_NAME = "desktopDatabase.db4o";
    public static final String MOBILE_DATABASE_NAME = "mobileDatabase.db4o";

    public static void main(String[] args) {
        oneWayReplicationExample();

        biDirectionalReplicationExample();

        selectiveReplicationByClass();
        selectiveReplicationWithCondition();
        selectiveReplicationWithQuery();

        deletionsReplication();
    }

    private static void oneWayReplicationExample() {
        deleteDatabases();
        storeObjectsIn(DESKTOP_DATABASE_NAME);

        //#example: Prepare unidirectional replication
        ObjectContainer desktopDatabase = openDatabase(DESKTOP_DATABASE_NAME);
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);
        
        ReplicationSession replicationSession = Replication.begin(providerA, providerB);
        
        // set the replication-direction from the desktop database to the mobile database. 
        replicationSession.setDirection(replicationSession.providerA(), replicationSession.providerB());
        //#end example:

        //#example: One direction replication
        ObjectSet changes = replicationSession.providerA().objectsChangedSinceLastReplication();
        for (Object changedObject : changes) { 
            replicationSession.replicate(changedObject);
        }
        replicationSession.commit();
        //#end example

        printCars(mobileDatabase);

        closeDBs(desktopDatabase, mobileDatabase);
    }

    private static void biDirectionalReplicationExample() {
        deleteDatabases();
        storeObjectsIn(DESKTOP_DATABASE_NAME);
        storeObjectsIn(MOBILE_DATABASE_NAME);

        // #example: Prepare bidirectional replication
        ObjectContainer desktopDatabase = openDatabase(DESKTOP_DATABASE_NAME);
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);
        
        ReplicationSession replicationSession = Replication.begin(providerA, providerB);
        // #end example

        //#example: Bidirectional replication
        // First get the changes of the two replication-partners
        ObjectSet changesOnDesktop = replicationSession.providerA().objectsChangedSinceLastReplication();
        ObjectSet changesOnMobile = replicationSession.providerB().objectsChangedSinceLastReplication();

        // then iterate over both change-sets and replicate it
        for (Object changedObjectOnDesktop : changesOnDesktop) {
            replicationSession.replicate(changedObjectOnDesktop);
        }

        for (Object changedObjectOnMobile : changesOnMobile) {
            replicationSession.replicate(changedObjectOnMobile);
        }

        replicationSession.commit();
        //#end example

        printCars(mobileDatabase);
        printCars(desktopDatabase);

        closeDBs(desktopDatabase, mobileDatabase);
    }

    private static void selectiveReplicationByClass() {
        deleteDatabases();
        storeObjectsIn(DESKTOP_DATABASE_NAME);

        ObjectContainer desktopDatabase = openDatabase(DESKTOP_DATABASE_NAME);
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);
        
        ReplicationSession replicationSession = Replication.begin(providerA, providerB);

        // #example: Selective replication by class
        ObjectSet changesOnDesktop =
            replicationSession.providerA().objectsChangedSinceLastReplication(Pilot.class);

        for (Object changedObjectOnDesktop : changesOnDesktop) {
            replicationSession.replicate(changedObjectOnDesktop);
        }

        replicationSession.commit();
        // #end example

        // the car's aren't replicated, only the pilots
        printCars(mobileDatabase);
        printPilots(mobileDatabase);

        closeDBs(desktopDatabase, mobileDatabase);
    }

    private static void selectiveReplicationWithCondition() {
        deleteDatabases();
        storeObjectsIn(DESKTOP_DATABASE_NAME);

        ObjectContainer desktopDatabase = openDatabase(DESKTOP_DATABASE_NAME);
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);
        
        ReplicationSession replicationSession = Replication.begin(providerA, providerB);

        // #example: Selective replication with a condition
        ObjectSet changesOnDesktop = replicationSession.providerA().objectsChangedSinceLastReplication();

        for (Object changedObjectOnDesktop : changesOnDesktop) {
            if (changedObjectOnDesktop instanceof Car) {
                if (((Car) changedObjectOnDesktop).getName().startsWith("M")) {
                    replicationSession.replicate(changedObjectOnDesktop);
                }
            }            
        }

        replicationSession.commit();
        // #end example

        // now only the cars which names start with "M" are replicated
        printCars(mobileDatabase);

        closeDBs(desktopDatabase, mobileDatabase);
    }

    private static void selectiveReplicationWithQuery() {
        deleteDatabases();
        storeObjectsIn(DESKTOP_DATABASE_NAME);

        ObjectContainer desktopDatabase = openDatabase(DESKTOP_DATABASE_NAME);
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);
        
        ReplicationSession replicationSession = Replication.begin(providerA, providerB);

        // #example: Selective replication with a query
        ObjectSet<Car> changesOnDesktop = desktopDatabase.query(new Predicate<Car>() {
            public boolean match(Car p) {
                return p.getName().startsWith("M");
            }
        });

        for (Car changedObjectOnDesktop  : changesOnDesktop) {
            replicationSession.replicate(changedObjectOnDesktop);
        }
        
        replicationSession.commit();
        // #end example

        // now only the cars which names start with "M" are replicated
        printCars(mobileDatabase);

        closeDBs(desktopDatabase, mobileDatabase);
    }

    private static void deletionsReplication() {
        deleteDatabases();
        storeObjectsIn(DESKTOP_DATABASE_NAME);
        ObjectContainer desktopDatabase = openDatabase(DESKTOP_DATABASE_NAME);
        ObjectContainer mobileDatabase = openDatabase(MOBILE_DATABASE_NAME);

        replicate(desktopDatabase, mobileDatabase);

        Car carToDelete = desktopDatabase.query(Car.class).get(0);
        desktopDatabase.delete(carToDelete);
        desktopDatabase.commit();

        printCars(mobileDatabase);

        // #example: Replicate deletions
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);
        
        ReplicationSession replicationSession = Replication.begin(providerA, providerB);
        replicationSession.replicateDeletions(Car.class);
        replicationSession.commit();
        // #end example

        printCars(mobileDatabase);

        closeDBs(desktopDatabase, mobileDatabase);

    }

    private static void replicate(ObjectContainer desktopDatabase, ObjectContainer mobileDatabase) {
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(desktopDatabase);
        Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(mobileDatabase);
        
        ReplicationSession replicationSession = Replication.begin(providerA, providerB);
        replicateChanges(replicationSession, replicationSession.providerA());
        replicateChanges(replicationSession, replicationSession.providerB());
        replicationSession.commit();
    }

    private static void replicateChanges(ReplicationSession replication, ReplicationProvider provider) {
        ObjectSet changes = provider.objectsChangedSinceLastReplication();
        for (Object changedObject : changes) {
            replication.replicate(changedObject);
        }
    }

    private static void closeDBs(ObjectContainer... databases) {
        for (ObjectContainer db : databases) {
            db.close();
        }
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

    private static ObjectContainer openDatabase(String fileName) {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Configure db4o to generate UUIDs and commit timestamps
        configuration.file().generateUUIDs(ConfigScope.GLOBALLY);
        configuration.file().generateCommitTimestamps(true);
        // #end example
        return Db4oEmbedded.openFile(configuration, fileName);
    }

    private static void deleteDatabases() {
        new File(DESKTOP_DATABASE_NAME).delete();
        new File(MOBILE_DATABASE_NAME).delete();
    }

}
