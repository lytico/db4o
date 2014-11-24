package com.db4o.drs.test.versant;

import static com.db4o.drs.test.versant.JavaLangUtils.*;

import java.io.*;
import java.net.*;
import java.util.*;

import javax.jdo.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.*;
import com.db4o.cs.config.*;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.test.versant.data.*;
import com.db4o.drs.versant.*;
import com.db4o.foundation.*;
import com.db4o.io.*;

import db4ounit.*;

public class ReplicationViaIntermediateServerTestCase extends VodProviderTestCaseBase {

    private int serverPort;
    
    private ObjectServer server;
    
    private ObjectContainer embedded;
    
    private ObjectContainer intermediateClient;
    
    private ObjectContainer intermediateServer; 

    public static void main(String[] args) {
        new ConsoleTestRunner(ReplicationViaIntermediateServerTestCase.class).run();
    }

    @Override
    public void setUp() {
        super.setUp();
        openServer();
        openEmbeddedRemoteDB();
    }
    
    @Override
    public void tearDown() {
    	super.tearDown();
    	if(embedded != null){
    		embedded.close();
    	}
    	if(intermediateClient != null){
    		intermediateClient.close();
    	}
    	if(intermediateServer != null){
    		intermediateServer.close();
    	}
    	if(server != null){
    		server.close();
    	}
    }

    public void testReplicateViaServer() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                replicateViaServer();
                return null;
            }
        });
    }

    private void replicateViaServer() {
        storeAndReplicate();
        storeAndReplicate();
        assertHasAmoutOfItems(2);
    }

    public void testReplicateDuringReplication() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                replicateDuringReplication();
                return null;
            }
        });
    }

    private void replicateDuringReplication() {
        storeAndReplicate();
        storeItemOnEmbedded();
        replicateIntermediateAndServer(new ReplicationAction() {
            @Override
            public void beforeCommit() {
                replicateEmbeddedAndIntermediate();
            }
        });
        replicateIntermediateAndServer();

        assertHasAmoutOfItems(2);
    }
    public void testReplicateOverlappingReplicationCommits() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                replicateOverlappingReplicationCommits();
                return null;
            }
        });
    }

    private void replicateOverlappingReplicationCommits() {
        storeItemOnEmbedded();
        storeItemOnEmbedded();
        storeItemOnEmbedded();

        final ReplicationProvider embedded = openEmbeddedReplicationProvider();
        final ReplicationProvider intermediate = openClientProvider();
        final ReplicationSession session = Replication.begin(embedded, intermediate);
        replicateIntermediateAndServer(new ReplicationAction() {
            @Override
            public void beforeCommit() {
                replicate(embedded, intermediate,ReplicationAction.NO_ACTIONS);
            }
            @Override
            public void afterCommit() {
                session.commit();
            }
        });
        replicateIntermediateAndServer();

        assertHasAmoutOfItems(3);
    }
    public void testReplicationOverlappingAndSpreadAcross() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                replicationOverlappingAndSpreadAcross();
                return null;
            }
        });
    }
    private void replicationOverlappingAndSpreadAcross() {
        storeItemOnEmbedded();
        storeItemOnEmbedded();
        storeItemOnEmbedded();

        final ReplicationProvider embedded = openEmbeddedReplicationProvider();
        final  ReplicationProvider intermediate = openClientProvider();
        final ReplicationSession session = Replication.begin(embedded, intermediate);
        final ObjectSet toReplicate = embedded.objectsChangedSinceLastReplication();
        session.replicate(toReplicate.get(0));
		replicateIntermediateAndServer(new ReplicationAction() {
		    @Override
		    public void beforeCommit() {
		        session.replicate(toReplicate.get(1));
		    }
		    @Override
		    public void afterCommit() {
		        session.replicate(toReplicate.get(2));
		        session.commit();
		    }
		});
        replicateIntermediateAndServer();
        assertHasAmoutOfItems(3);
    }

    private void assertHasAmoutOfItems(final int count) {
        JDOUtilities.withTransaction(_vod.persistenceManagerFactory(), new JDOTransactionClosure() {
            public void invoke(PersistenceManager transaction) {
                List<Item> item = (List<Item>) transaction.newQuery(Item.class).execute();
                Assert.areEqual(count, item.size());
            }
        });
    }

    private void storeAndReplicate() {
        storeItemOnEmbedded();
        replicateEmbeddedAndIntermediate();
        replicateIntermediateAndServer();
    }

    private void openEmbeddedRemoteDB() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configureReplication(configuration);
        this.embedded = Db4oEmbedded.openFile(configuration, "!In:Memory!");
    }

    private void openServer() {
        ServerConfiguration config = Db4oClientServer.newServerConfiguration();
        configureReplication(config);
        this.serverPort = findFreePort();
        this.server = Db4oClientServer.openServer(config, "localhost", serverPort);
        this.server.grantAccess("","");
    }

    private void configureReplication(FileConfigurationProvider config) {
        config.file().storage(new MemoryStorage());
        config.file().generateCommitTimestamps(true);
        config.file().generateUUIDs(ConfigScope.GLOBALLY);
    }

    private void replicateIntermediateAndServer() {
        replicateIntermediateAndServer(ReplicationAction.NO_ACTIONS);
    }

    private void replicateIntermediateAndServer(ReplicationAction replicationActions) {
        ReplicationProvider intermediate = openServerProvider();
        VodReplicationProvider server = openVodReplicationProvider();
        replicate(intermediate, server,replicationActions);
    }

    private void replicateEmbeddedAndIntermediate() {
        ReplicationProvider embedded = openEmbeddedReplicationProvider();
        ReplicationProvider intermediate = openClientProvider();

        replicate(embedded, intermediate,ReplicationAction.NO_ACTIONS);
    }


    private void storeItemOnEmbedded() {
        embedded.store(new Item("Item"));
        embedded.commit();
    }

    private VodReplicationProvider openVodReplicationProvider() {
        VodReplicationProvider provider = new VodReplicationProvider(_vod);
        provider.listenForReplicationEvents(Holder.class, Item.class);
        return provider;
    }

    private ReplicationProvider openServerProvider() {
    	if(intermediateServer != null){
    		intermediateServer.close();
    	}
        intermediateServer = Db4oClientServer.openClient("localhost",serverPort, "","");
		return new Db4oClientServerReplicationProvider(
                intermediateServer);
    }
    private ReplicationProvider openClientProvider() {
    	if(intermediateClient != null){
    		intermediateClient.close();
    	}
        intermediateClient = Db4oClientServer.openClient("localhost",serverPort, "","");
		return new Db4oClientServerReplicationProvider(intermediateClient);
    }
    private ReplicationProvider openEmbeddedReplicationProvider() {
        return new Db4oEmbeddedReplicationProvider(embedded);
    }

    public static int findFreePort() {
        int port = 0;
        try {
            ServerSocket server =
                    new ServerSocket(0);
            port = server.getLocalPort();
            server.close();
            return port;
        } catch (IOException e) {
            throw reThrow(e);
        }
    }
}

class MutableReference<T>{
    private T value;

    public T getValue() {
        return value;
    }

    public void setValue(T value) {
        this.value = value;
    }

    public static  <T>  MutableReference<T> create() {
        return new MutableReference<T>();
    }
}

class JavaLangUtils{

    static RuntimeException reThrow(Exception e) {
        JDOUtilities.<RuntimeException>throwsUnchecked(e);
        throw new RuntimeException(e);
    }

    static <T extends Exception> void throwsUnchecked(Exception toThrow) throws T{
        // Since the type is erased, this cast actually does nothing!!!
        // we can throw any exception
        throw (T) toThrow;
    }
}