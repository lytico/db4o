package com.db4odoc.clientserver.referencecache;

import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;

import java.io.File;


public class ReferenceCacheExamples {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUp();
        ObjectServer server = Db4oClientServer.openServer(DATABASE_FILE,1337);
        try{
            server.grantAccess("sa","sa");
            storeData(server);

            referenceCacheExample();
            unitOfWork();
        } finally {
            server.close();
        }
    }

    private static void referenceCacheExample() {
        ObjectContainer client1
                = Db4oClientServer.openClient("localhost",1337,"sa","sa");
        ObjectContainer client2
                = Db4oClientServer.openClient("localhost",1337,"sa","sa");
        try{
            // #example: Reference cache in client server
            Person personOnClient1 = queryForPerson(client1);
            Person personOnClient2 = queryForPerson(client2);
            System.out.println(queryForPerson(client2).getName());

            personOnClient1.setName("New Name");
            client1.store(personOnClient1);
            client1.commit();

            // The other client still has the old data in the cache
            System.out.println(queryForPerson(client2).getName());

            client2.ext().refresh(personOnClient2,Integer.MAX_VALUE);

            // After refreshing the date is visible
            System.out.println(queryForPerson(client2).getName());
            // #end example
        } finally {
            client1.close();
            client2.close();
        }
    }

    private static void unitOfWork() {
        ObjectContainer client
                = Db4oClientServer.openClient("localhost",1337,"sa","sa");
        try{
            // #example: Clean cache for each unit of work
            ObjectContainer container = client.ext().openSession();
            try{
                // do work
            }finally {
                container.close();
            }
            // Start with a fresh cache:
            container = client.ext().openSession();
            try{
                // do work
            }finally {
                container.close();
            }
            // #end example
        } finally {
            client.close();
        }
    }

    private static Person queryForPerson(ObjectContainer container) {
        return container.query(Person.class).get(0);
    }

    private static void storeData(ObjectServer server) {
        ObjectContainer container = server.openClient();
        try{
            container.store(new Person("Joe"));
        } finally {
            container.close();
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
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
