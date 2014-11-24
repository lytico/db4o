package com.db4odoc.drs.rdms;

import java.io.*;

import org.hibernate.*;
import org.hibernate.cfg.Configuration;
import org.hibernate.classic.Session;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.hibernate.*;
import com.db4o.drs.hibernate.impl.*;


public class Db4oRDBMSReplicationExamples {
	
    static final String DB4O_DATABASE_FILE = "database.db4o";
    
    public static void main(String[] args) throws Exception {
        fromDb4oToHibernate();
        bidirectionalReplication();
    }

    private static void fromDb4oToHibernate() {
        cleanUp();
        storeTestDataInDb4o();

        // #example: Prepare the Hibernate configuration
        Configuration hibernateConfig
                = new Configuration().configure("com/db4odoc/drs/rdms/hibernate.cfg.xml");
        ReplicationConfigurator.configure(hibernateConfig);
        // #end example

        // #example: Prepare replication
        ObjectContainer container = openDB();
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(container);
        HibernateReplicationProvider providerB = new HibernateReplicationProvider(hibernateConfig);
        
        ReplicationSession replicationSession = Replication.begin(providerA, providerB);
        replicationSession.setDirection(replicationSession.providerA(),replicationSession.providerB());
        // #end example
        
        // #example: Replicate from db4o to Hibernate
        final ObjectSet changesInHibernate =
                replicationSession.providerA().objectsChangedSinceLastReplication();
        for (Object changedObject : changesInHibernate) {
            replicationSession.replicate(changedObject);
        }

        replicationSession.commit();
        // #end example
    }
    private static void bidirectionalReplication() {
        cleanUp();
        storeTestDataInDb4o();
        storeTestDataInHibernate();

        // #example: Bidirectional replication
        Configuration hibernateConfig
                = new Configuration().configure("com/db4odoc/drs/rdms/hibernate.cfg.xml");
        ObjectContainer container = openDB();
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(container);
        HibernateReplicationProvider providerB = new HibernateReplicationProvider(hibernateConfig);
        
        ReplicationSession replicationSession = Replication.begin(providerA, providerB);

        final ObjectSet changesInDb4o = replicationSession.providerA()
                .objectsChangedSinceLastReplication();
        for (Object changedObject : changesInDb4o) {
            replicationSession.replicate(changedObject);
        }
        final ObjectSet changesInHibernate = replicationSession.providerB()
                .objectsChangedSinceLastReplication();
        for (Object changedObject : changesInHibernate) {
            replicationSession.replicate(changedObject);
        }
        // #end example

        replicationSession.commit();
    }
    private static void replicateCollections() {
        cleanUp();
        storeTestDataInDb4o();
        storeTestDataInHibernate();

        // #example: Bidirectional replication
        Configuration hibernateConfig
                = new Configuration().configure("com/db4odoc/drs/rdms/hibernate.cfg.xml");
        ObjectContainer container = openDB();
        
        Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(container);
        HibernateReplicationProvider providerB = new HibernateReplicationProvider(hibernateConfig);
        
        ReplicationSession replicationSession = Replication.begin(providerA, providerB);

        final ObjectSet changesInDb4o = replicationSession.providerA()
                .objectsChangedSinceLastReplication();
        for (Object changedObject : changesInDb4o) {
            replicationSession.replicate(changedObject);
        }
        final ObjectSet changesInHibernate = replicationSession.providerB()
                .objectsChangedSinceLastReplication();
        for (Object changedObject : changesInHibernate) {
            replicationSession.replicate(changedObject);
        }
        // #end example

        replicationSession.commit();
    }

    private static void storeTestDataInDb4o() {
        ObjectContainer container = openDB();
        try{
            container.store(new Car(new Pilot("John",42),"VM Gold"));
            container.store(new Car(new Pilot("Joanna",55),"VM Passat"));
        }finally {
            container.close();
        }
    }

    private static void storeTestDataInHibernate() {
        Configuration hibernateConfig
                = new Configuration().configure("com/db4odoc/drs/rdms/hibernate.cfg.xml");
        ReplicationConfigurator.configure(hibernateConfig);

        // #example: Install the listeners to the session
        SessionFactory sessionFactory = hibernateConfig.buildSessionFactory();
        Session session = sessionFactory.openSession();
        ReplicationConfigurator.install(session, hibernateConfig);
        // #end example
    }

    private static void cleanUp() {
        new File(DB4O_DATABASE_FILE).delete();
    }

    private static ObjectContainer openDB() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Configure db4o to generate UUIDs and commit timestamps
        configuration.file().generateUUIDs(ConfigScope.GLOBALLY);
        configuration.file().generateCommitTimestamps(true);
        // #end example
        return Db4oEmbedded.openFile(configuration,DB4O_DATABASE_FILE);
    }
}
