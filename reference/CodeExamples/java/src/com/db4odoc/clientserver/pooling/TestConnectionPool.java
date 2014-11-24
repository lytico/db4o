package com.db4odoc.clientserver.pooling;

import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ServerConfiguration;
import com.db4o.io.MemoryStorage;
import org.junit.After;
import org.junit.Before;
import org.junit.Test;

import static junit.framework.Assert.assertEquals;
import static org.junit.Assert.assertNotNull;
import static org.junit.Assert.assertNotSame;


public class TestConnectionPool {
    private static final int PORT = 1337;
    private static final String USER_AND_PASSWORD = "sa";

    private ObjectServer server;
    private ConnectionPool toTest;
    private AssertionConnectionFactory assertConnectionFactory;

    @Before
    public void setup() {
        this.server = createInMemoryServer();
        storeTestObjects();
        this.assertConnectionFactory = newFactory();
        this.toTest = new ConnectionPool(assertConnectionFactory);
    }


    @After
    public void tearDown() {
        this.server.close();
    }

    @Test
    public void returnsConnection() {
        ConnectionPool toTest = new ConnectionPool(newFactory());
        ObjectContainer container = toTest.acquire();
        assertNotNull(container);
    }

    @Test
    public void createsConnectionForSecondAquire() {
        ObjectContainer container1 = toTest.acquire();
        ObjectContainer container2 = toTest.acquire();
        assertConnectionFactory.assertWasCalledTimes(2);
    }


    @Test
    public void closeAndRelease() {
        ObjectContainer container1 = toTest.acquire();
        container1.store(new AStoredObject());
        toTest.closeAndRelease(container1);
        assertEquals(2,this.server.openClient().query(AStoredObject.class).size());
    }

    @Test
    public void reusesContainerIfReleased() {
        ObjectContainer container1 = toTest.acquire();
        toTest.release(container1);
        ObjectContainer container2 = toTest.acquire();
        assertConnectionFactory.assertWasCalledTimes(1);
    }

    @Test(expected = IllegalArgumentException.class)
    public void cannotMultipleReturnClient() {
        ObjectContainer container1 = toTest.acquire();
        toTest.release(container1);
        toTest.release(container1);
    }



    @Test()
    public void reusedClientIsNotTainted() {
        ObjectContainer container1 = toTest.acquire();
        final AStoredObject fromContainer1 = testObjectFromContainer(container1);
        toTest.release(container1);
        final AStoredObject fromContainer2 = testObjectFromContainer(toTest.acquire());
        assertNotSame(fromContainer1,fromContainer2);
    }
    @Test()
    public void nowGhostCommits() {
        ObjectContainer container1 = toTest.acquire();
        container1.store(new AStoredObject());
        toTest.release(container1);
        ObjectContainer container2 = toTest.acquire();
        container2.commit();

        final int countStoredObjects = this.server.openClient().query(AStoredObject.class).size();
        assertEquals(1,countStoredObjects);
    }
    @Test()
    public void canPassClosedContainers() {
        ObjectContainer container = toTest.acquire();
        container.close();
        toTest.release(container);
    }


    @Test()
    public void noTransactionSharing() {
        final ObjectContainer session1 = toTest.acquire();
        final ObjectContainer session2 = toTest.acquire();
        session1.store(new AStoredObject());
        final int count1 = session1.query(AStoredObject.class).size();
        session2.rollback();

        final int count2 = session1.query(AStoredObject.class).size();
        assertEquals(2,count1);
        assertEquals(2,count2);

    }

    private AStoredObject testObjectFromContainer(ObjectContainer container1) {
        return container1.query(AStoredObject.class).get(0);
    }


    private void storeTestObjects() {
        final ObjectContainer objectContainer = server.openClient();
        objectContainer.store(new AStoredObject());
        objectContainer.close();
    }

    private AssertionConnectionFactory newFactory() {
        return new AssertionConnectionFactory();
    }

    private static class AssertionConnectionFactory implements ClientConnectionFactory {
        private int wasCalled;
        @Override
        public ObjectContainer connect() {
            this.wasCalled++;
            return Db4oClientServer.openClient("localhost", PORT, USER_AND_PASSWORD, USER_AND_PASSWORD);
        }


        public void assertWasCalledTimes(int times) {
            assertEquals(times, wasCalled);
        }


    }


    private ObjectServer createInMemoryServer() {
        ServerConfiguration config = Db4oClientServer.newServerConfiguration();
        config.file().storage(new MemoryStorage());
        ObjectServer server = Db4oClientServer.openServer(config, "In:Memory", PORT);
        server.grantAccess(USER_AND_PASSWORD, USER_AND_PASSWORD);
        return server;
    }


    private class AStoredObject{
        
    }
}
