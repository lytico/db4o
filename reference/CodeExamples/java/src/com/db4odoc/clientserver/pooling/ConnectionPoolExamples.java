package com.db4odoc.clientserver.pooling;

import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;


class ConnectionPoolExamples {
    private static final int PORT = 1337;
    private static final String USER_AND_PASSWORD = "sa";


    public static void main(String[] args) {
        ObjectServer server = startServer();

        ConnectionPool connectionPool = new ConnectionPool(new ClientConnectionFactory() {
            @Override
            public ObjectContainer connect() {
                // #example: Open clients for the pool
                ObjectContainer client = Db4oClientServer.openClient("localhost",PORT,
                                USER_AND_PASSWORD, USER_AND_PASSWORD);
                // #end example
                return client;
            }
        });

        useThePool(connectionPool);

        server.close();
    }

    private static void useThePool(ConnectionPool connectionPool) {
        final ObjectContainer session = connectionPool.acquire();
        try{
           session.store(new Person("Joe"));
        } finally {
            connectionPool.closeAndRelease(session);
        }
    }

    private static ObjectServer startServer() {
        ObjectServer server = Db4oClientServer.openServer("In:Memory", PORT);
        server.grantAccess(USER_AND_PASSWORD, USER_AND_PASSWORD);
        return server;
    }

    static class Person{
        private String name;

        Person(String name) {
            this.name = name;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }
}
