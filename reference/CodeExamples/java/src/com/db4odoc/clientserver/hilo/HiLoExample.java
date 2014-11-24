package com.db4odoc.clientserver.hilo;

import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;

public class HiLoExample {
    private static final int PORT = 1337;
    private static final String USER_NAME = "sa";
    private static final String PASSWORD = "sa";

    public static void main(String[] args) {
        ObjectServer server = startServer();

        ObjectContainer client1 = openClient();
        HiLoIdSupport.install(client1);

        for(int i=0;i<1000;i++){

        final WithID idObject = new WithID();
        client1.store(idObject);
            System.out.println("Id generated: "+idObject.getId());
        }


        server.close();
    }

    private static ObjectContainer openClient() {
        return Db4oClientServer.openClient("localhost",PORT, USER_NAME, PASSWORD);
    }

    private static ObjectServer startServer() {
        ObjectServer server = Db4oClientServer.openServer("database.db4o", PORT);
        server.grantAccess(USER_NAME, PASSWORD);
        return server;
    }

    static class WithID extends IdHolder{

    }
}
