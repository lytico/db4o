package com.db4odoc.configuration.client;

import com.db4o.ObjectContainer;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ClientConfiguration;


public class ClientConfigurationExamples {


    public static void prefetchDepth(){
        // #example: Configure the prefetch depth
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.prefetchDepth(5);
        // #end example
        ObjectContainer container = Db4oClientServer.openClient(configuration, "localhost",1337,"user","password");
        container.close();

    }
    public static void prefetchObjectCount(){
        // #example: Configure the prefetch object count
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.prefetchObjectCount(500);
        // #end example
        ObjectContainer container = Db4oClientServer.openClient(configuration, "localhost",1337,"user","password");
        container.close();

    }
    public static void prefetchSlotCacheSize(){
        // #example: Configure the slot cache
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.prefetchSlotCacheSize(1024);
        // #end example
        ObjectContainer container = Db4oClientServer.openClient(configuration, "localhost",1337,"user","password");
        container.close();

    }
    public static void prefetchIDCount(){
        // #example: Configure the prefetch id count
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.prefetchIDCount(128);
        // #end example
        ObjectContainer container = Db4oClientServer.openClient(configuration, "localhost",1337,"user","password");
        container.close();

    }
    public static void connectionTimeOut(){
        // #example: Configure the timeout
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.timeoutClientSocket(1*60*1000);
        // #end example
        ObjectContainer container = Db4oClientServer.openClient(configuration, "localhost",1337,"user","password");
        container.close();

    }
}
