package com.db4odoc.disconnectedobj.objectidentity;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.query.Predicate;

import java.io.File;

public class ObjectIdentityExamples {
    private static final String DATABASE_FILE_NAME = "database.db4o";


    public static void main(String[] args) {
        updateWorksOnSameContainer();
        newObjectIsStoredIfDifferentContainer();

    }

    private static void newObjectIsStoredIfDifferentContainer() {
        cleanUp();
        storeJoe();

        // #example: Update doesn't works when using the different object containers
        {
            ObjectContainer container = openDatabase();
            Pilot joe = queryByName(container,"Joe");
            container.close();

            // The update on another object container
            ObjectContainer otherContainer = openDatabase();
            joe.setName("Joe New");
            otherContainer.store(joe);
            otherContainer.close();
        }
        {
            // instead of updating the existing pilot,
            // a new instance was stored.
            ObjectContainer container = openDatabase();
            ObjectSet<Pilot> pilots = container.query(Pilot.class);
            System.out.println("Amount of pilots: "+pilots.size());
            for (Pilot pilot : pilots) {
                System.out.println(pilot);
            }
            container.close();
        }
        // #end example

        cleanUp();
    }

    private static void updateWorksOnSameContainer() {
        cleanUp();
        storeJoe();

        // #example: Update works when using the same object container
        {
            ObjectContainer container = openDatabase();
            Pilot joe = queryByName(container,"Joe");
            joe.setName("Joe New");
            container.store(joe);
            container.close();
        }
        {
            ObjectContainer container = openDatabase();
            ObjectSet<Pilot> pilots = container.query(Pilot.class);
            System.out.println("Amount of pilots: "+pilots.size());
            for (Pilot pilot : pilots) {
                System.out.println(pilot);
            }
            container.close();
        }
        // #end example

        cleanUp();
    }

    private static Pilot queryByName(ObjectContainer container,final String name) {
        return container.query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot p) {
                return p.getName().equals(name);
            }
        }).get(0);
    }

    private static void storeJoe() {
        ObjectContainer container = openDatabase();
        container.store(new Pilot("Joe"));
        container.close();
    }


    private static void cleanUp() {
        new File(DATABASE_FILE_NAME).delete();
    }


    private static ObjectContainer openDatabase() {
        return Db4oEmbedded.openFile(DATABASE_FILE_NAME);
    }
}
