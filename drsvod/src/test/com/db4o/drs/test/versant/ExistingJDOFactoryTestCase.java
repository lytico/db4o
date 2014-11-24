/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.drs.test.versant;

import java.util.*;

import javax.jdo.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.test.versant.data.*;
import com.db4o.drs.versant.*;
import com.db4o.foundation.*;
import com.db4o.io.*;

import db4ounit.*;

public class ExistingJDOFactoryTestCase extends VodProviderTestCaseBase{

    public static void main(String[] args) {
        new ConsoleTestRunner(ExistingJDOFactoryTestCase.class).run();
    }

    public void testCanReplicateWithVODDatabaseFromJODManualImpl() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                PersistenceManagerFactory factory = createFactory();
                VodDatabase server = createDatabase(factory);

                canReplicateWithVODDatabaseFromJDO(server);
                return null;
            }
        });
    }

    public void testCanReplicateWithVODDatabaseFromJOD() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                PersistenceManagerFactory factory = createFactory();
                VodDatabase server = new VodDatabase(factory);
                canReplicateWithVODDatabaseFromJDO(server);
                return null;
            }
        });

    }

    private void canReplicateWithVODDatabaseFromJDO(VodDatabase server) {
        server.configureEventProcessor("localhost",_vod.eventProcessorPort());
        ObjectContainer client = newDb4oContainer();

        VodReplicationProvider serverProvider = new VodReplicationProvider(server);

        serverProvider.listenForReplicationEvents(Item.class,Holder.class);
        ReplicationProvider clientProvider = new Db4oEmbeddedReplicationProvider(client);

        ReplicationSession session = Replication.begin(serverProvider, clientProvider);
        session.close();
        client.close();

    }

    public static VodDatabase createDatabase(PersistenceManagerFactory sessionFactory){
        Properties properties = sessionFactory.getProperties();
        String connectionURL = properties.getProperty("javax.jdo.option.ConnectionURL");
        if(isEmpty(connectionURL) || notVersantDBConnection(connectionURL)){
            throw new IllegalArgumentException("Requires a valid database connection URL for VOD");
        }
        return new VodDatabase(extractName(connectionURL),properties);
    }
    
    private static String extractName(String connectionURL) {
        return connectionURL.substring("versant:".length()).split("@")[0];
    }
    
    private static boolean notVersantDBConnection(String connectionURL) {
        return !connectionURL.startsWith("versant");
    }

    private static boolean isEmpty(String connectionURL) {
        return null==connectionURL || 0==connectionURL.trim().length();
    }

    private ObjectContainer newDb4oContainer() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().storage(new MemoryStorage());
        return Db4oEmbedded.openFile(configuration, "!In!Memory!");
    }

    private static PersistenceManagerFactory createFactory() {
    	Properties properties = new Properties();
    	properties.setProperty("javax.jdo.PersistenceManagerFactoryClass", "com.versant.core.jdo.BootstrapPMF");
    	properties.setProperty(VodDatabase.CONNECTION_URL_KEY, "versant:VodProviderTestCaseBase");
    	properties.setProperty(VodDatabase.PASSWORD_KEY, "drs");
    	properties.setProperty(VodDatabase.USER_NAME_KEY, "drs");
    	properties.setProperty("versant.metadata.0", "com/db4o/drs/test/versant/data/package.jdo");
    	return JDOHelper.getPersistenceManagerFactory(properties);
    }
}
