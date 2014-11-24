package com.db4odoc.basics;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.query.Predicate;

import java.util.List;


public class Db4oBasics {
    public static void main(String[] args) {
        openAndCloseTheContainer();
        storeObject();
        query();
        updateDatabase();
        storeObject();
        deleteObject();

        allOperationsInOnGo();
    }

    private static void storeObject() {
        // #example: Store an object
        ObjectContainer container = Db4oEmbedded.openFile("databaseFile.db4o");
        try {
            Pilot pilot = new Pilot("Joe");
            container.store(pilot);
        } finally {
            container.close();
        }
        // #end example
    }

    private static void query() {
        // #example: Query for objects
        ObjectContainer container = Db4oEmbedded.openFile("databaseFile.db4o");
        try {
            List<Pilot> pilots = container.query(new Predicate<Pilot>() {
                public boolean match(Pilot o) {
                    return o.getName().equals("Joe");
                }
            });
            for (Pilot pilot : pilots) {
                System.out.println(pilot.getName());
            }
        } finally {
            container.close();
        }
        // #end example
    }

    private static void updateDatabase() {
        // #example: Update a pilot
        ObjectContainer container = Db4oEmbedded.openFile("databaseFile.db4o");
        try {
            List<Pilot> pilots = container.query(new Predicate<Pilot>() {
                public boolean match(Pilot o) {
                    return o.getName().equals("Joe");
                }
            });
            Pilot aPilot = pilots.get(0);
            aPilot.setName("New Name");
            // update the pilot
            container.store(aPilot);
        } finally {
            container.close();
        }
        // #end example
    }

    private static void deleteObject() {
        // #example: Delete a object
        ObjectContainer container = Db4oEmbedded.openFile("databaseFile.db4o");
        try {
            List<Pilot> pilots = container.query(new Predicate<Pilot>() {
                public boolean match(Pilot o) {
                    return o.getName().equals("Joe");
                }
            });
            Pilot aPilot = pilots.get(0);
            container.delete(aPilot);
        } finally {
            container.close();
        }
        // #end example
    }

    private static void openAndCloseTheContainer() {
        // #example: Open the object container to use the database
        ObjectContainer container = Db4oEmbedded.openFile("databaseFile.db4o");
        try {
            // use the object container
        } finally {
            container.close();
        }
        // #end example
    }

    private static void allOperationsInOnGo() {
        // #example: The basic operations
        ObjectContainer container = Db4oEmbedded.openFile("databaseFile.db4o");
        try {
            // store a new pilot
            Pilot pilot = new Pilot("Joe");
            container.store(pilot);

            // query for pilots
            List<Pilot> pilots = container.query(new Predicate<Pilot>() {
                @Override
                public boolean match(Pilot pilot) {
                    return pilot.getName().startsWith("Jo");
                }
            });

            // update pilot
            Pilot toUpdate = pilots.get(0);
            toUpdate.setName("New Name");
            container.store(toUpdate);

            // delete pilot
            container.delete(toUpdate);
        } finally {
            container.close();
        }
        // #end example
    }
}
