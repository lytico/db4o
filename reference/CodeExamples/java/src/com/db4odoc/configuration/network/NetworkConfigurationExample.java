package com.db4odoc.configuration.network;

import com.db4o.ObjectContainer;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ClientConfiguration;
import com.db4o.cs.foundation.StandardSocket4Factory;
import com.db4o.cs.internal.config.StandardClientServerFactory;


public class NetworkConfigurationExample {

    private static void enableBatchMode(){
        // #example: enable or disable batch mode
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.networking().batchMessages(true);
        // #end example
        ObjectContainer container = Db4oClientServer.openClient(configuration, "localhost", 1337, "sa", "sa");
    }

    private static void replaceClientServerFactory(){
        // #example: exchange the way a client or server is created
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.networking().clientServerFactory(new StandardClientServerFactory());
        // #end example
        ObjectContainer container = Db4oClientServer.openClient(configuration, "localhost", 1337, "sa", "sa");
    }

    private static void maxBatchQueueSize(){
        // #example: change the maximum batch queue size
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.networking().maxBatchQueueSize(1024);
        // #end example
        ObjectContainer container = Db4oClientServer.openClient(configuration, "localhost", 1337, "sa", "sa");
    }

    private static void singleThreadedClient(){
        // #example: single threaded client
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.networking().singleThreadedClient(true);
        // #end example
        ObjectContainer container = Db4oClientServer.openClient(configuration, "localhost", 1337, "sa", "sa");
    }

    private static void pluggableSocket(){
        // #example: Exchange the socket-factory
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.networking().socketFactory(new StandardSocket4Factory());
        // #end example
        ObjectContainer container = Db4oClientServer.openClient(configuration, "localhost", 1337, "sa", "sa");
    }

}
