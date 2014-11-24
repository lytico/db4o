package com.db4odoc.callbacks.eventregistry;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;
import com.db4o.events.*;
import com.db4o.foundation.Iterator4;
import com.db4o.internal.FrozenObjectInfo;
import com.db4o.internal.LazyObjectReference;
import com.db4o.query.Predicate;

import java.io.File;

public class EventRegistryExamples {
    private static final String DATABASE_FILE_NAME = "database.db4o";
    private static final int PORT_NUMBER = 1337;
    private static final String EMBEDDED_USER = "user";
    private static final String EMBEDDED_PASSWORD = "user";

    public static void main(String[] args) {
        System.out.println("--Events in embedded mode--");
        eventsInLocalContainer();
        System.out.println("--Events in client/server mode--");
        eventsClientServer();
        System.out.println("--Cancel in event --");
        cancelInEvent();
        System.out.println("--Commit-events --");
        commitEvents();
    }


    private static void eventsInLocalContainer() {
        cleanUp();
        storeJoe();

        ObjectContainer container = openEmbedded();
        // #example: Obtain the event-registry
        EventRegistry events = EventRegistryFactory.forObjectContainer(container);
        // #end example

        registerAFewEvents(events, "local embedded container");
        runOperations(container);

        container.close();
        cleanUp();
    }

    private static void registerForEventsOnTheServer(){
        // #example: register for events on the server
        ObjectServer server = 
                Db4oClientServer.openServer(DATABASE_FILE_NAME, PORT_NUMBER);
        EventRegistry eventsOnServer =
                EventRegistryFactory.forObjectContainer(server.ext().objectContainer());    
        // #end example
    }

    private static void eventsClientServer() {
        cleanUp();
        storeJoe();

        ObjectServer server = openServer();
        EventRegistry eventsOnServer = EventRegistryFactory.forObjectContainer(server.ext().objectContainer());
        registerAFewEvents(eventsOnServer,  "db4o server");

        ObjectContainer client1 = openClient();
        EventRegistry eventsOnClient1 = EventRegistryFactory.forObjectContainer(client1);
        registerAFewEvents(eventsOnClient1,  "db4o client 1");
        runOperations(client1);


        ObjectContainer client2 = openClient();
        EventRegistry eventsOnClient2 = EventRegistryFactory.forObjectContainer(client2);
        registerAFewEvents(eventsOnClient2,  "db4o client 2");

        sleepForAWhile();
        client1.close();
        client2.close();
        server.close();
        
        cleanUp();

    }

    private static void cancelInEvent() {
        cleanUp();
        storeJoe();

        ObjectContainer container = openEmbedded();
        // #example: Cancel store operation
        EventRegistry events = EventRegistryFactory.forObjectContainer(container);
        events.creating().addListener(new EventListener4<CancellableObjectEventArgs>() {
            public void onEvent(Event4<CancellableObjectEventArgs> events,
                                CancellableObjectEventArgs eventArgs) {
                if(eventArgs.object() instanceof Person){
                    Person p = (Person) eventArgs.object();
                    if(p.getName().equals("Joe Junior")){
                        eventArgs.cancel();
                    }
                }
            }
        });
        // #end example
        container.store(new Person("Joe Junior"));

        int personCount = container.query(Person.class).size();
        System.out.println("Only "+personCount+" because store was cancelled");

        container.close();
        cleanUp();
    }
    private static void commitEvents() {
        cleanUp();
        storeJoe();

        ObjectContainer container = openEmbedded();
        // #example: Commit-info
        EventRegistry events = EventRegistryFactory.forObjectContainer(container);
        events.committed().addListener(new EventListener4<CommitEventArgs>() {
            public void onEvent(Event4<CommitEventArgs> events,
                                CommitEventArgs eventArgs) {
                for(Iterator4 it=eventArgs.added().iterator();it.moveNext();){
                    LazyObjectReference reference = (LazyObjectReference) it.current();
                    System.out.println("Added "+reference.getObject());
                }
                for(Iterator4 it=eventArgs.updated().iterator();it.moveNext();){
                    LazyObjectReference reference = (LazyObjectReference) it.current();
                    System.out.println("Updated "+reference.getObject());
                }
                for(Iterator4 it=eventArgs.deleted().iterator();it.moveNext();){
                    FrozenObjectInfo deletedInfo = (FrozenObjectInfo) it.current();
                    // the deleted info might doesn't contain the object anymore and
                    // return the null.
                    System.out.println("Deleted "+deletedInfo.getObject());
                }
            }
        });
        // #end example
        runOperations(container);

        container.close();
        cleanUp();
    }

    private static void runOperations(ObjectContainer container) {
        Person joe = container.query(new Predicate<Person>() {
            @Override
            public boolean match(Person p) {
                return p.getName().equals("Joe");
            }
        }).get(0);
        joe.setName("Joe Senior");
        container.store(joe);
        container.store(new Person("Joe Junior"));
        container.commit();
    }

    private static void storeJoe() {
        ObjectContainer container = openEmbedded();
        container.store(new Person("Joe"));
        container.close();
    }

    private static void registerAFewEvents(EventRegistry events, final String containerName) {
        events.activating().addListener(new EventListener4<CancellableObjectEventArgs>() {
            public void onEvent(Event4<CancellableObjectEventArgs> cancellableObjectEventArgsEvent4,
                                CancellableObjectEventArgs cancellableObjectEventArgs) {
                System.out.println("Activating on "+containerName);
            }
        });
        events.activated().addListener(new EventListener4<ObjectInfoEventArgs>() {
            public void onEvent(Event4<ObjectInfoEventArgs> objectInfoEventArgsEvent4,
                                ObjectInfoEventArgs objectInfoEventArgs) {
                System.out.println("Activated on "+containerName);
            }
        });
        events.creating().addListener(new EventListener4<CancellableObjectEventArgs>() {
            public void onEvent(Event4<CancellableObjectEventArgs> cancellableObjectEventArgsEvent4,
                                CancellableObjectEventArgs cancellableObjectEventArgs) {
                System.out.println("Creating on "+containerName);
            }
        });
        events.created().addListener(new EventListener4<ObjectInfoEventArgs>() {
            public void onEvent(Event4<ObjectInfoEventArgs> objectInfoEventArgsEvent4,
                                ObjectInfoEventArgs objectInfoEventArgs) {
                System.out.println("Created on "+containerName);
            }
        });
        events.updating().addListener(new EventListener4<CancellableObjectEventArgs>() {
            public void onEvent(Event4<CancellableObjectEventArgs> cancellableObjectEventArgsEvent4,
                                CancellableObjectEventArgs cancellableObjectEventArgs) {
                System.out.println("Updating on "+containerName);
            }
        });
        events.updated().addListener(new EventListener4<ObjectInfoEventArgs>() {
            public void onEvent(Event4<ObjectInfoEventArgs> objectInfoEventArgsEvent4,
                                ObjectInfoEventArgs objectInfoEventArgs) {
                System.out.println("Updated on "+containerName);
            }
        });
        events.queryStarted().addListener(new EventListener4<QueryEventArgs>() {
            public void onEvent(Event4<QueryEventArgs> queryEventArgsEvent4, QueryEventArgs queryEventArgs) {
                System.out.println("Query started on "+containerName);
            }
        });
        events.queryFinished().addListener(new EventListener4<QueryEventArgs>() {
            public void onEvent(Event4<QueryEventArgs> queryEventArgsEvent4, QueryEventArgs queryEventArgs) {
                System.out.println("Query finished on "+containerName);
            }
        });
        events.committing().addListener(new EventListener4<CommitEventArgs>() {
            public void onEvent(Event4<CommitEventArgs> commitEventArgsEvent4,
                                CommitEventArgs commitEventArgs) {
                System.out.println("committing on "+containerName);
            }
        });
        events.committed().addListener(new EventListener4<CommitEventArgs>() {
            public void onEvent(Event4<CommitEventArgs> commitEventArgsEvent4,
                                CommitEventArgs commitEventArgs) {
                System.out.println("Committed on "+containerName);
            }
        });
        // #example: register for a event
        events.committing().addListener(new EventListener4<CommitEventArgs>() {
            public void onEvent(Event4<CommitEventArgs> source,
                                CommitEventArgs arguments) {
                handleCommitting(source,arguments);
            }
        });
        // #end example
    }

    // #example: implement your event handling
    private static void handleCommitting(Event4<CommitEventArgs> source,
                                         CommitEventArgs commitEventArgs) {
        // handle the event here
    }
    // #end example



    private static ObjectContainer openEmbedded() {
        return Db4oEmbedded.openFile(DATABASE_FILE_NAME);
    }
    private static ObjectContainer openClient() {
        return Db4oClientServer.openClient("localhost", PORT_NUMBER, EMBEDDED_USER, EMBEDDED_PASSWORD);
    }

    private static ObjectServer openServer() {
        ObjectServer server = Db4oClientServer.openServer(DATABASE_FILE_NAME, PORT_NUMBER);
        server.grantAccess(EMBEDDED_USER, EMBEDDED_PASSWORD);
        return server;
    }

    private static void cleanUp() {
        new File(DATABASE_FILE_NAME).delete();
    }

    private static void sleepForAWhile() {
        try {
            Thread.sleep(2000);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }

}
