package com.db4odoc.drs.vod;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.ConfigScope;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.cs.Db4oClientServer;
import com.db4o.drs.Replication;
import com.db4o.drs.ReplicationProvider;
import com.db4o.drs.ReplicationSession;
import com.db4o.drs.db4o.Db4oClientServerReplicationProvider;
import com.db4o.drs.db4o.Db4oEmbeddedReplicationProvider;

public class ClientReplicator {
    public static void main(String[] args) {
        // #example: Replicate against the db4o intermediate server
        EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        config.file().generateUUIDs(ConfigScope.GLOBALLY);
        config.file().generateCommitTimestamps(true);
        ObjectContainer theMobileDatabase = Db4oEmbedded.openFile(config, "mobileDatabase.db4o");
        ObjectContainer db4oServer = Db4oClientServer.openClient("localhost", 8080, "sa", "sa");
        try {
            ReplicationProvider localProvider = new Db4oEmbeddedReplicationProvider(theMobileDatabase);
            ReplicationProvider remoteProvider = new Db4oClientServerReplicationProvider(db4oServer);

            final ReplicationSession replicationSession = Replication.begin(localProvider, remoteProvider);

            replicateAll(replicationSession,localProvider.objectsChangedSinceLastReplication());
            replicateAll(replicationSession,remoteProvider.objectsChangedSinceLastReplication());

            replicationSession.commit();

        } catch (Exception e){
            throw new RuntimeException(e);
        } finally{
            db4oServer.close();
            theMobileDatabase.close();
        }
        // #end example
    }

    private static void replicateAll(ReplicationSession replicationSession, ObjectSet objectSet) {
        for (Object obj : objectSet) {
            replicationSession.replicate(obj);
        }
    }
}
