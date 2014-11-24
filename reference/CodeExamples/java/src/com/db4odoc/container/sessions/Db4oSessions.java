package com.db4odoc.container.sessions;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.ObjectSet;
import com.db4o.cs.Db4oClientServer;

import java.io.File;


public class Db4oSessions {
    private static final String DATABASE_FILE_NAME = "database.db4o";

    public static void main(String[] args) {
        sessions();
        sessionsIsolation();
        sessionCache();
        embeddedClient();
    }

    public static void sessions() {
        cleanUp();
        // #example: Session object container
        ObjectContainer rootContainer = Db4oEmbedded.openFile(DATABASE_FILE_NAME);

        // open the db4o-session. For example at the beginning for a web-request
        ObjectContainer session = rootContainer.ext().openSession();
        try {
            // do the operations on the session-container
            session.store(new Person("Joe"));
        } finally {
            // close the container. For example when the request ends
            session.close();
        }
        // #end example

        rootContainer.close();
    }

    private static void sessionsIsolation() {
        cleanUp();
        ObjectContainer rootContainer = Db4oEmbedded.openFile(DATABASE_FILE_NAME);

        ObjectContainer session1 = rootContainer.ext().openSession();
        ObjectContainer session2 = rootContainer.ext().openSession();
        try {
            // #example: Session are isolated from each other
            session1.store(new Person("Joe"));
            session1.store(new Person("Joanna"));

            // the second session won't see the changes until the changes are committed
            printAll(session2.query(Person.class));

            session1.commit();

            // new the changes are visiable for the second session
            printAll(session2.query(Person.class));
            // #end example
        } finally {
            // close the container. For example when the request ends
            session1.close();
            session2.close();
        }

        rootContainer.close();
    }

    private static void sessionCache() {
        cleanUp();
        ObjectContainer rootContainer = Db4oEmbedded.openFile(DATABASE_FILE_NAME);

        ObjectContainer session1 = rootContainer.ext().openSession();
        ObjectContainer session2 = rootContainer.ext().openSession();
        try {
            storeAPerson(session1);

            // #example: Each session does cache the objects
            Person personOnSession1 = session1.query(Person.class).get(0);
            Person personOnSession2 = session2.query(Person.class).get(0);

            personOnSession1.setName("NewName");
            session1.store(personOnSession1);
            session1.commit();


            // the second session still sees the old value, because it was cached
            System.out.println(personOnSession2.getName());
            // you can explicitly refresh it
            session2.ext().refresh(personOnSession2, Integer.MAX_VALUE);
            System.out.println(personOnSession2.getName());
            // #end example
        } finally {
            // close the container. For example when the request ends
            session1.close();
            session2.close();
        }

        rootContainer.close();
    }

    private static void storeAPerson(ObjectContainer session1) {
        session1.store(new Person("Joe"));
        session1.commit();
    }

    public static void embeddedClient() {
        cleanUp();
        // #example: Embedded client
        ObjectServer server = Db4oClientServer.openServer(DATABASE_FILE_NAME, 0);

        // open the db4o-embedded client. For example at the beginning for a web-request
        ObjectContainer container = server.openClient();
        try {
            // do the operations on the session-container
            container.store(new Person("Joe"));
        } finally {
            // close the container. For example when the request ends
            container.close();
        }
        // #end example

        server.close();
    }

    private static void printAll(ObjectSet<Person> persons) {
        for (Person person : persons) {
            System.out.println(person);
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE_NAME).delete();
    }


    private static class Person {
        private String name;

        private Person(String name) {
            this.name = name;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        @Override
        public String toString() {
            return "Person{" +
                    "name='" + name + '\'' +
                    '}';
        }
    }
}
