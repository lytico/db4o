using System;
using System.Collections.Generic;
using Db4objects.Db4o;

namespace Db4oDoc.Code.ClientServer.Pooling
{
    public delegate IObjectContainer ClientConnectionFactory();

    public class ConnectionPool {
    private readonly ClientConnectionFactory connectionFactory;
    private readonly Queue<IObjectContainer> availableClients = new Queue<IObjectContainer>();
    private readonly IDictionary<IObjectContainer, IObjectContainer> leasedClients = new Dictionary<IObjectContainer, IObjectContainer>();
    private readonly object sync = new object();

    public ConnectionPool(ClientConnectionFactory connectionFactory) {
        this.connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Get a object-container from the pool. This pool either uses an existing client
    /// or opens a new connection. The connection is opened with the
    /// given <see cref="ClientConnectionFactory"/> 
    /// </summary>
    /// <returns>A object container from the pool</returns>
    public IObjectContainer Acquire() {
        lock (sync){
            SessionClientPair containerPair = AcquirePooledContainer();
            leasedClients.Add(containerPair.SessionContainer,containerPair.ClientContainer);
            return containerPair.SessionContainer;
        }
    }

    /// <summary>
    /// Return the object container to the pool. If you haven't committed the changes yet
    /// they changes will be lost. Use <see cref="IObjectContainer.Close"/> or <see cref="IObjectContainer.Commit"/>
    /// before using this method to commit the changes
    /// </summary>
    /// <param name="session">The container to return to the pool</param>
    public void Release(IObjectContainer session) {
        lock (sync){
            EnsureIsLegitimateContainer(session);
            ReturnToPool(session);
        }
    }

    /// <summary>
    /// Close and return the object-container. Will commit the changes first and the
    /// return the object container to the pool
    /// </summary>
    /// <param name="session">The container to return to the pool</param>
    public void CloseAndRelease(IObjectContainer session){
        session.Dispose();
        Release(session);
    }

    private SessionClientPair AcquirePooledContainer() {
        // #example: Obtain a pooled container
        // Obtain a client container. Either take one from the pool or allocate a new one
        IObjectContainer client = ObtainClientContainer();
        // Make sure that the transaction is in clean state
        client.Rollback();
        // Then create a session on that client container and use it for the database operations.
        // The client-container is now in use. Ensure that it isn't leased twice.
        IObjectContainer sessionContainer = client.Ext().OpenSession();
        // #end example
        return new SessionClientPair(sessionContainer,client);
    }


    private IObjectContainer ObtainClientContainer() {
        if(availableClients.Count>0){
            return availableClients.Dequeue();
        }
        return connectionFactory();
    }

    private void ReturnToPool(IObjectContainer session) {
        // #example: Returning to pool
        // First you need to get the underlying client for the session
        IObjectContainer client = ClientForSession(session);
        // then the client is ready for reuse
        ReturnToThePool(client);
        // #end example
        leasedClients.Remove(session);
    }

    private void ReturnToThePool(IObjectContainer client) {
        availableClients.Enqueue(client);
    }

    private IObjectContainer ClientForSession(IObjectContainer session) {
        return leasedClients[session];
    }

    private void EnsureIsLegitimateContainer(IObjectContainer container) {
        if(!leasedClients.ContainsKey(container)){
            throw new ArgumentException("You cannot return a container which isn't leased");
        }
    }

    struct SessionClientPair{
        private readonly IObjectContainer sessionContainer;
        private readonly IObjectContainer clientContainer;

        public SessionClientPair(IObjectContainer sessionContainer,
                                  IObjectContainer clientContainer) {
            this.sessionContainer = sessionContainer;
            this.clientContainer = clientContainer;
        }

        public IObjectContainer SessionContainer
        {
            get { return sessionContainer; }
        }

        public IObjectContainer ClientContainer
        {
            get { return clientContainer; }
        }
    }
}

}