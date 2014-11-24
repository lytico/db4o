package com.db4odoc.clientserver.refresh;

import com.db4o.*;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ServerConfiguration;
import com.db4o.events.*;
import com.db4o.foundation.Iterator4;
import com.db4o.internal.LazyObjectReference;

import java.io.File;
import java.util.ArrayList;
import java.util.List;


public class RefreshingObjects {
    private static final String DATABASE_FILE_NAME = "database.db4o";
    private static final String USER_AND_PASSWORD = "sa";
    private static final int PORT_NUMBER = 1337;


    public static void main(String[] args) {
        useEventsToRefreshObjects();
        refreshOnDemand();
    }

    private static void refreshOnDemand() {
        cleanUp();


        ObjectServer server = openServer();
        server.grantAccess(USER_AND_PASSWORD, USER_AND_PASSWORD);
        storeJoeOnOtherClient();


        ObjectContainer client = openClient();
        List<Person> allPersons = listAllPersons(client);
        printPersons(allPersons);

        updateJoeOnOtherClient();

        // the persons are not in the most current state
        printPersons(allPersons);

        // but you can explicitly refresh the objects
        refresh(client, allPersons);
        printPersons(allPersons);


        waitForALittleWhile();
        server.close();
        cleanUp();
    }

    private static void refresh(ObjectContainer db,List<Person> allPersons) {
        for (Person objToRefresh : allPersons) {
            // #example: refresh a object
            db.ext().refresh(objToRefresh,Integer.MAX_VALUE);
            // #end example
        }
    }

    private static void useEventsToRefreshObjects() {
        cleanUp();

        ObjectServer server = openServer();
        server.grantAccess(USER_AND_PASSWORD, USER_AND_PASSWORD);
        storeJoeOnOtherClient();


        ObjectContainer client = openClient();
        registerEvent(client);
        List<Person> allPersons = listAllPersons(client);
        printPersons(allPersons);

        updateJoeOnOtherClient();

        // the events are asynchronously transported over the network
        // which takes a while
        waitForALittleWhile();
        printPersons(allPersons);


        waitForALittleWhile();
        server.close();
        cleanUp();
    }

    private static void registerEvent(ObjectContainer container) {
        // #example: On the updated-event we refresh the objects
        EventRegistry events = EventRegistryFactory.forObjectContainer(container);
        events.committed().addListener(new EventListener4<CommitEventArgs>() {
            public void onEvent(Event4<CommitEventArgs> commitEvent, CommitEventArgs commitEventArgs) {
                for(Iterator4 it = commitEventArgs.updated().iterator();it.moveNext();){
                    LazyObjectReference reference = (LazyObjectReference) it.current();
                    Object obj = reference.getObject();
                    commitEventArgs.objectContainer().ext().refresh(obj,1);
                }
            }
        });
        // #end example
    }

    private static void printPersons(List<Person> allPersons) {
        for (Person person : allPersons) {
            System.out.println(person);
        }
    }

    private static void storeJoeOnOtherClient() {
        ObjectContainer client = openClient();
        client.store(new Person("Joe"));
        client.close();
    }

    private static void updateJoeOnOtherClient() {
        ObjectContainer container = openClient();
        ObjectSet<Person> persons = container.query(Person.class);
        for (Person person : persons) {
            person.setName("New "+person.getName());
            container.store(person);
        }
        container.close();
    }


    private static List<Person> listAllPersons(ObjectContainer container) {
        ObjectSet<Person> persons = container.query(Person.class);
        return new ArrayList<Person>(persons);
    }

    private static void waitForALittleWhile() {
        try {
            Thread.sleep(200);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }

    private static ObjectContainer openClient() {
        return Db4oClientServer.openClient("localhost", PORT_NUMBER, USER_AND_PASSWORD, USER_AND_PASSWORD);
    }


    private static ObjectServer openServer() {
        ServerConfiguration configuration = Db4oClientServer.newServerConfiguration();
        return Db4oClientServer.openServer(configuration, DATABASE_FILE_NAME, PORT_NUMBER);
    }

    private static void cleanUp() {
        new File(DATABASE_FILE_NAME).delete();
    }

}
