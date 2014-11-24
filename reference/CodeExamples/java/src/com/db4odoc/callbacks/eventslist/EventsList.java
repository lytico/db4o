package com.db4odoc.callbacks.eventslist;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.ObjectSet;
import com.db4o.cs.Db4oClientServer;
import com.db4o.events.*;
import com.db4o.ext.ExtClient;
import com.db4o.query.Predicate;


public class EventsList {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        prepareData();
        System.out.println("====>Events in local container====");
        doLocalOperations();
        System.out.println("====>Events in client-server====");
        doClientServer();

    }

    private static void doLocalOperations() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            registerAllEvents("local-container",container);

            doDBOperations(container);

        } finally {
            container.close();
        }
    }

    private static void doClientServer() {
        ObjectServer server = Db4oClientServer.openServer(DATABASE_FILE,1337);
        server.grantAccess("sa","sa");
        try {
//            registerAllEvents("db4o-server",server.ext().objectContainer());
//
            doClientOperations();

        } finally {
            server.close();
        }
    }

    private static void doClientOperations() {
        ObjectContainer container = Db4oClientServer.openClient("localhost",1337,"sa","sa");
        try {
            registerAllEvents("Client-container",container);

            doDBOperations(container);
            container.commit();
            waitALittle();
        } finally {
            container.close();
        }
    }

    private static void prepareData() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        container.store(new TestObject());
        container.close();
    }

    private static void doDBOperations(ObjectContainer container) {
        TestObject obj = new TestObject();
        container.store(obj);
        container.ext().deactivate(obj);

        final ObjectSet<TestObject> data = container.query(new Predicate<TestObject>() {
            @Override
            public boolean match(TestObject testObject) {
                return true;
            }
        });
        obj = data.get(0);
        container.store(obj);

        container.delete(obj);

    }

    private static void registerAllEvents(final String containerName,ObjectContainer container) {
        EventRegistry events = EventRegistryFactory.forObjectContainer(container);
        events.activating().addListener(singObjectCancellableEvent(containerName, "activating"));
        events.activated().addListener(singleObjectEvent(containerName, "activated"));
        events.creating().addListener(singObjectCancellableEvent(containerName, "creating"));
        events.created().addListener(singleObjectEvent(containerName, "created"));
        if(!(container instanceof ExtClient)){
            events.deleting().addListener(singObjectCancellableEvent(containerName, "deleting"));
            events.deleted().addListener(singleObjectEvent(containerName, "deleted"));
        }
        events.updating().addListener(singObjectCancellableEvent(containerName, "updating"));
        events.updated().addListener(singleObjectEvent(containerName, "updated"));
        events.deactivating().addListener(singObjectCancellableEvent(containerName, "deactivating"));
        events.deactivated().addListener(singleObjectEvent(containerName, "deactivated"));
        events.queryStarted().addListener(queryEvent(containerName, "queryStarted"));
        events.queryFinished().addListener(queryEvent(containerName, "queryFinished"));
        events.committing().addListener(commitEvent(containerName, "committing"));
        events.committed().addListener(commitEvent(containerName, "committed"));
        events.opened().addListener(objectContainerEvent(containerName, "opened"));
        events.closing().addListener(objectContainerEvent(containerName, "closing"));
        events.classRegistered().addListener(classRegisteredEvent(containerName, "classRegistered"));
        events.instantiated().addListener(singleObjectEvent(containerName, "instantiated"));
    }

    private static EventListener4<ClassEventArgs> classRegisteredEvent(
            String container,
            String eventName) {
        return newEventHandler(container, eventName);
    }

    private static EventListener4<ObjectContainerEventArgs> objectContainerEvent(
            String container,
            String eventName) {
        return newEventHandler(container, eventName);
    }

    private static EventListener4<CancellableObjectEventArgs> singObjectCancellableEvent(
            String container,
            final String eventName) {
        return newEventHandler(container, eventName);
    }

    private static EventListener4<ObjectInfoEventArgs> singleObjectEvent(
            String container,
            final String eventName) {
        return newEventHandler(container, eventName);
    }

    private static EventListener4<QueryEventArgs> queryEvent(
            String container,
            final String eventName) {
        return newEventHandler(container, eventName);
    }

    private static EventListener4<CommitEventArgs> commitEvent(
            String container,
            final String eventName) {
        return newEventHandler(container, eventName);
    }


    private static <T extends EventArgs> EventListener4<T> newEventHandler(
            final String container, final String eventName) {
        EventListener4<EventArgs> eventHandler = new EventListener4<EventArgs>() {
            @Override
            public void onEvent(Event4<EventArgs> eventInfo,
                                EventArgs arguments) {
                System.out.println(container+": "+eventName);
            }
        };
        return (EventListener4<T>) eventHandler;
    }

    private static void waitALittle() {
        try{
            Thread.sleep(1000);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }

    static class TestObject {
        private int number = 42;
    }
}

