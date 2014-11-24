package com.db4odoc.clientserver.basics;

import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;


public class ServerExample {
    public static void main(String[] args) throws Exception {
        // #example: Start a db4o server
        ObjectServer server = Db4oClientServer.openServer("database.db4o",8080);
        try{
            server.grantAccess("user","password");

            // Let the server run.
            letServerRun();
        } finally {
            server.close();
        }
        // #end example


    }

    private static void letServerRun() throws Exception {
        System.out.println("Press a key to close the server");
        while(System.in.available()==0){
            Thread.sleep(1000);
        }
    }
}
