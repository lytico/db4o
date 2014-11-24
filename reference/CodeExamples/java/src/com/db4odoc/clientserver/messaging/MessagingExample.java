package com.db4odoc.clientserver.messaging;

import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ClientConfiguration;
import com.db4o.cs.config.ServerConfiguration;
import com.db4o.messaging.MessageContext;
import com.db4o.messaging.MessageRecipient;
import com.db4o.messaging.MessageSender;


public class MessagingExample {
    private static final String DATABASE_FILE = "database.db4o";
    private static final int PORT_NUMBER = 1337;
    private static final String USER_AND_PASSWORD = "sa";

    public static void main(String[] args) {
        messagingExample();
    }

    private static void messagingExample() {
        ObjectServer server = startUpServer();

        // #example: configure a message receiver for a client
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.networking().messageRecipient(new MessageRecipient() {
            public void processMessage(MessageContext messageContext, Object o) {
                System.out.println("The client received a '" + o + "' message");
            }
        });
        // #end example

        // #example: Get the message sender and use it
        MessageSender sender = configuration.messageSender();
        ObjectContainer container = Db4oClientServer.openClient(configuration, "localhost", PORT_NUMBER, USER_AND_PASSWORD, USER_AND_PASSWORD);


        sender.send(new HelloMessage("Hi Server!"));
        // #end example

        waitForAWhile();
        container.close();


        server.close();
    }

    private static void waitForAWhile() {
        try {
            Thread.sleep(2000);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }

    private static ObjectServer startUpServer() {
        // #example: configure a message receiver for the server
        ServerConfiguration configuration = Db4oClientServer.newServerConfiguration();
        configuration.networking().messageRecipient(new MessageRecipient() {
            public void processMessage(MessageContext messageContext, Object o) {
                System.out.println("The server received a '" + o + "' message");
                // you can respond to the client
                messageContext.sender().send(new HelloMessage("Hi Client!"));
            }
        });
        ObjectServer server = Db4oClientServer.openServer(configuration, DATABASE_FILE, PORT_NUMBER);
        // #end example
        server.grantAccess(USER_AND_PASSWORD, USER_AND_PASSWORD);
        return server;
    }


}
