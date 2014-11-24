package com.db4odoc.clientserver.basics;

import com.db4o.ObjectContainer;
import com.db4o.cs.Db4oClientServer;

public class Db4oClientExample {
    public static void main(String[] args) {
        // #example: Connect to the server
        final ObjectContainer container
                = Db4oClientServer.openClient("localhost", 8080, "user", "password");
        try{
            // Your operations
        }finally {
            container.close();
        }
        // #end example
    }
}
