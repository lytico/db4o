package com.db4odoc.configuration.server;

import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ServerConfiguration;


public class ServerConfigurationExamples {

    private static void socketTimeout(){
        // #example: configure the socket-timeout
        ServerConfiguration configuration = Db4oClientServer.newServerConfiguration();
        configuration.timeoutServerSocket(10*60*1000);
        // #end example

        ObjectServer container = Db4oClientServer.openServer(configuration, "database.db4o",1337);

        container.close();
    }
}
