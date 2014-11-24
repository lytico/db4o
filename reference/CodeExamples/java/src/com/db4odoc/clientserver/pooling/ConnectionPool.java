package com.db4odoc.clientserver.pooling;

import com.db4o.ObjectContainer;

import java.util.ArrayDeque;
import java.util.HashMap;
import java.util.Map;
import java.util.Queue;


public class ConnectionPool {
    private final ClientConnectionFactory connectionFactory;
    private final Queue<ObjectContainer> availableClients = new ArrayDeque<ObjectContainer>();
    private final Map<ObjectContainer,ObjectContainer> leasedClients = new HashMap<ObjectContainer,ObjectContainer>();
    private final Object sync = new Object();

    public ConnectionPool(ClientConnectionFactory connectionFactory) {
        this.connectionFactory = connectionFactory;
    }

    /**
     * Get a object-container from the pool. This pool either uses an existing client
     * or opens a new connection. The connection is opened with the
     * given {@link com.db4odoc.clientserver.pooling.ClientConnectionFactory}
     * @return A object container from the pool
     */
    public ObjectContainer acquire() {
        synchronized (sync){
            SessionClientPair containerPair = acquirePooledContainer();
            leasedClients.put(containerPair.sessionContainer,containerPair.clientContainer);
            return containerPair.sessionContainer;
        }
    }

    /**
     * Return the object container to the pool. If you haven't committed the changes yet
     * they changes will be lost. Use {@link com.db4o.ObjectContainer#close()} or {@link com.db4o.ObjectContainer#commit()}
     * before using this method to commit the changes
     * @param session The container to return to the pool
     */
    public void release(ObjectContainer session) {
        synchronized (sync){
            ensureIsLegitimateContainer(session);
            returnToPool(session);
        }
    }

    /**
     * Close and return the object-container. Will commit the changes first and the
     * return the object container to the pool
     * @param session The container to return to the pool
     */
    public void closeAndRelease(ObjectContainer session){
        session.close();
        release(session);
    }

    private SessionClientPair acquirePooledContainer() {
        // #example: Obtain a pooled container
        // Obtain a client container. Either take one from the pool or allocate a new one
        ObjectContainer client = obtainClientContainer();
        // Make sure that the transaction is in clean state
        client.rollback();
        // Then create a session on that client container and use it for the database operations.
        // The client-container is now in use. Ensure that it isn't leased twice.
        final ObjectContainer sessionContainer = client.ext().openSession();
        // #end example
        return new SessionClientPair(sessionContainer,client);
    }


    private ObjectContainer obtainClientContainer() {
        if(!availableClients.isEmpty()){
            return availableClients.poll();
        }
        return connectionFactory.connect();
    }

    private void returnToPool(ObjectContainer session) {
        // #example: Returning to pool
        // First you need to get the underlying client for the session
        final ObjectContainer client = clientForSession(session);
        // then the client is ready for reuse
        returnToThePool(client);
        // #end example
        leasedClients.remove(session);
    }

    private void returnToThePool(ObjectContainer client) {
        availableClients.offer(client);
    }

    private ObjectContainer clientForSession(ObjectContainer session) {
        return leasedClients.get(session);
    }

    private void ensureIsLegitimateContainer(ObjectContainer container) {
        if(!leasedClients.containsKey(container)){
            throw new IllegalArgumentException("You cannot return a container which isn't leased");
        }
    }

    private static class SessionClientPair{
        private final ObjectContainer sessionContainer;
        private final ObjectContainer clientContainer;

        private SessionClientPair(ObjectContainer sessionContainer,
                                  ObjectContainer clientContainer) {
            this.sessionContainer = sessionContainer;
            this.clientContainer = clientContainer;
        }
    }
}
