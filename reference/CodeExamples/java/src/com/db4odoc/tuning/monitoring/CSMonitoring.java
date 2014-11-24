package com.db4odoc.tuning.monitoring;

import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.ObjectSet;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ClientConfiguration;
import com.db4o.cs.config.ServerConfiguration;
import com.db4o.cs.monitoring.ClientConnectionsMonitoringSupport;
import com.db4o.cs.monitoring.NetworkingMonitoringSupport;

import java.io.IOException;
import java.util.Random;


public class CSMonitoring {
    private static final int PORT_NUMBER = 1337;
    private static final String USER = "sa";
    private static final String PASSWORD = "pwd";

    public static void main(String[] args) throws IOException {
        ObjectServer server = startServer();


        runClient();

        server.close();
    }

    private static void runClient() throws IOException {
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.common().add(new NetworkingMonitoringSupport());
        ObjectContainer client = Db4oClientServer.openClient(configuration, "localhost",PORT_NUMBER, USER, PASSWORD);
     
        doOperationsOnClient(client);
        client.close();
    }

    private static void doOperationsOnClient(ObjectContainer container) throws IOException
        {
        while(System.in.available()==0){
            storeALot(container);
            readALot(container);
        }
    }

    private static void readALot(ObjectContainer container) {
        final ObjectSet<DataObject> allObjects = container.query(DataObject.class);
        for (DataObject object : allObjects) {
            object.toString();
        }
    }

    private static void storeALot(ObjectContainer container) {
        Random rnd = new Random();
        for(int i=0;i<1024;i++){
            container.store(new DataObject(rnd));
        }
        container.commit();
    }

    private static ObjectServer startServer() {
        ServerConfiguration configuration = Db4oClientServer.newServerConfiguration();
        // #example: Add the network monitoring support
        configuration.common().add(new NetworkingMonitoringSupport());
        // #end example
        // #example: Add the client connections monitoring support
        configuration.addConfigurationItem(new ClientConnectionsMonitoringSupport());
        // #end example
        ObjectServer server =Db4oClientServer.openServer(configuration, "database.db4o", PORT_NUMBER);
        server.grantAccess(USER,PASSWORD);
        return server;
    }

}
