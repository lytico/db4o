package com.db4o.drs.test.versant;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.ConfigScope;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.drs.Replication;
import com.db4o.drs.ReplicationProvider;
import com.db4o.drs.ReplicationSession;
import com.db4o.drs.db4o.Db4oEmbeddedReplicationProvider;
import com.db4o.drs.versant.VodReplicationProvider;
import com.db4o.drs.versant.ipc.tcp.*;
import com.db4o.drs.versant.jdo.reflect.JdoReflector;
import com.db4o.io.MemoryStorage;
import db4ounit.Assert;
import db4ounit.ConsoleTestRunner;
import db4ounit.TestLifeCycle;

import java.util.concurrent.*;

/**
 * @author roman.stoffel@gamlor.info
 * @since 08.12.10
 */
public class ReplicationTimeoutTestCase extends VodDatabaseTestCaseBase implements TestLifeCycle {

    private static final int TIMEOUT_IN_SECONDS = 30;
    private ExecutorService executor = createExecutors();

    public static void main(String[] args) {
        new ConsoleTestRunner(ReplicationTimeoutTestCase.class).run();
    }

    public void testTimeoutsIfNoProcessorIsRunning() throws Exception {
        ExecutorService executor = this.executor;
        Future<Boolean> timeoutAwaiter = executor.submit(new Callable<Boolean>() {
            public Boolean call() throws Exception {
                return timeoutWhileTryingToConnect();
            }
        });
        try {
            boolean operationResult = timeoutAwaiter.get(TIMEOUT_IN_SECONDS, TimeUnit.SECONDS);
            Assert.isTrue(operationResult);
        } catch (TimeoutException e) {
            Assert.fail("Expect a timeout from the test-case!", e);
        }
    }

    public void setUp() throws Exception {
        _vod.createEventSchema();
        _vod.startEventDriver();
        // No event processor! This test is about what happens when the stuff isn't setup right
    }

    public void tearDown() throws Exception {
        _vod.stopEventDriver();
        _vod.stopEventProcessor();
    }

    private boolean timeoutWhileTryingToConnect() {
        ReplicationProvider vodReplicationPartner = new VodReplicationProvider(_vod);

        Db4oEmbeddedReplicationProvider db4oReplicationPartner = newDB4OPartner();
        try {
	        try {
	            ReplicationSession replication =
	                    Replication.begin(db4oReplicationPartner, vodReplicationPartner);
	            replication.commit();
	        } catch (Exception e) {
	            Assert.isTrue(e.getCause() instanceof ConnectionTimeoutException);
	            return true;
	        }
	        return false;
        } finally {
        	if (db4oReplicationPartner != null) {
        		db4oReplicationPartner.getObjectContainer().close();
        	}
        }
    }

    private Db4oEmbeddedReplicationProvider newDB4OPartner() {
        EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        config.common().reflectWith(
                new JdoReflector(Thread.currentThread().getContextClassLoader()));
        config.file().generateUUIDs(ConfigScope.GLOBALLY);
        config.file().generateVersionNumbers(ConfigScope.GLOBALLY);
        config.file().storage(new MemoryStorage());
        ObjectContainer container = Db4oEmbedded.openFile(config, "In:Memory");
        return new Db4oEmbeddedReplicationProvider(container);
    }

    private ExecutorService createExecutors() {
        return Executors.newCachedThreadPool(new ThreadFactory() {
            public Thread newThread(Runnable r) {
                Thread workerThread = Executors.defaultThreadFactory().newThread(r);
                workerThread.setDaemon(true);
                return workerThread;
            }
        });
    }

}