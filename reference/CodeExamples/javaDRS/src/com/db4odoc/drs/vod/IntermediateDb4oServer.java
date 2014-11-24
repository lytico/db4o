package com.db4odoc.drs.vod;


import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.config.ConfigScope;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ClientConfiguration;
import com.db4o.cs.config.ServerConfiguration;
import com.db4o.drs.Replication;
import com.db4o.drs.ReplicationProvider;
import com.db4o.drs.ReplicationSession;
import com.db4o.drs.db4o.Db4oClientServerReplicationProvider;
import com.db4o.drs.versant.VodDatabase;
import com.db4o.drs.versant.VodReplicationProvider;
import com.db4o.drs.versant.jdo.reflect.JdoReflector;
import com.db4o.io.PagingMemoryStorage;

import javax.jdo.PersistenceManagerFactory;
import java.util.Timer;
import java.util.TimerTask;

public class IntermediateDb4oServer {
    private static final int TEN_SECONDS_IN_MILLISEC = 10 * 1000;
    private static final int ONE_HOUR_IN_MILLISEC = 60 * 60 * 1000;
    private final PersistenceManagerFactory factory;

    public static void main(String[] args) {

        PersistenceManagerFactory factory = JDOUtilities.createPersistenceFactory();
        try{
            new IntermediateDb4oServer(factory).main();
        } finally {
            factory.close();
        }
    }

    public IntermediateDb4oServer(PersistenceManagerFactory factory) {
        this.factory = factory;
    }

    private void main() {
        ObjectServer server = startupServer();
        try{
            // #example: Schedule every 10 seconds
            final Timer timer = new Timer(true);
            timer.schedule(new TimerTask() {
                        @Override
                        public void run() {
                            runReplication();
                        }
                    },0, TEN_SECONDS_IN_MILLISEC);
            // #end example
            try {
                // We just let this server run for an hour
                Thread.sleep(ONE_HOUR_IN_MILLISEC);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        } finally{
            server.close();
        }
    }
    // #example: Replicate with the db4o server
    private void runReplication() {
        ClientConfiguration config = Db4oClientServer.newClientConfiguration();
        config.common().reflectWith(new JdoReflector(Thread.currentThread().getContextClassLoader()));
        ObjectContainer intermediate = Db4oClientServer.openClient(config,"localhost",8080,"sa","sa");
            ReplicationProvider mobileDatabase
                = new Db4oClientServerReplicationProvider(intermediate);
        // The rest is just the regular replication stuff
        // #end example
        VodReplicationProvider centralDatabase = createVODReplicator();
        try{

            ReplicationSession replicationSession =
                Replication.begin(mobileDatabase, centralDatabase);

            for (Object changedObject : mobileDatabase.objectsChangedSinceLastReplication()) {
                replicationSession.replicate(changedObject);
            }
            for (Object changedObject : centralDatabase.objectsChangedSinceLastReplication()) {
                replicationSession.replicate(changedObject);
            }
            replicationSession.commit();
        } finally {
            intermediate.close();
        }
    }

    private VodReplicationProvider createVODReplicator() {
        VodDatabase vodDatabase = new VodDatabase(factory);
        vodDatabase.configureEventProcessor("localhost",4088);
        VodReplicationProvider centralDatabase
                = new VodReplicationProvider(vodDatabase);
        centralDatabase.listenForReplicationEvents(Pilot.class);
        return new VodReplicationProvider(vodDatabase);
    }

    private ObjectServer startupServer() {
        // #example: Setup server
        ServerConfiguration config = Db4oClientServer.newServerConfiguration();
        config.file().generateUUIDs(ConfigScope.GLOBALLY);
        config.file().generateCommitTimestamps(true);
        config.file().storage(new PagingMemoryStorage());
        config.common().reflectWith(new JdoReflector(Thread.currentThread().getContextClassLoader()));
        ObjectServer server = Db4oClientServer.openServer(config, "!In:Memory!", 8080);
        server.grantAccess("sa", "sa");
        // #end example
        return server;
    }
}
