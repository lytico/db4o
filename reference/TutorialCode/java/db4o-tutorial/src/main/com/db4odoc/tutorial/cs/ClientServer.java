package com.db4odoc.tutorial.cs;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;

public class ClientServer {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) throws Exception {
        sessionContainers();
        clientServer();
        startServer();
    }

    private static void sessionContainers() {
        ObjectContainer rootContainer = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Creating a session container
            ObjectContainer container = rootContainer.ext().openSession();
            try{
                // We now can use this session container like any other container
            } finally {
                container.close();
            }
            // #end example
        } finally {
            rootContainer.close();
        }
    }

    private static void startServer() throws Exception {
        // #example: Open server
        ObjectServer server = Db4oClientServer.openServer("database.db4o",8080);
        try{
            // allow access to this server
            server.grantAccess("user","password");

            // Keep server running as long as you need it
            System.out.println("Press any key to exit.");
            System.in.read();
            System.out.println("Exiting...");
        }finally {
            server.close();
        }
        // #end example
    }

    private static void clientServer() {
        ObjectServer server = Db4oClientServer.openServer("database.db4o",8080);
        try{
            server.grantAccess("user","password");

            openClient();
        }finally {
            server.close();
        }
    }

    private static void openClient() {
        // #example: Using the client
        ObjectContainer container
                = Db4oClientServer.openClient("localhost",8080,"user","password");
        try{
            // Use the client object container as usual
        } finally {
            container.close();
        }
        // #end example
    }
}
