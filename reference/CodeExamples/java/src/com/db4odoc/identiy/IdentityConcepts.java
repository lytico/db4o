package com.db4odoc.identiy;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;

import java.io.File;


public class IdentityConcepts {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUp();
        storeAObject();
        referenceEquals();

        storeAndLoadWithTheSame();
        storeOnDifferentContainers();

        removeFromReferenceCache();

    }

    private static void referenceEquals() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: db4o ensures reference equality
            final Car theCar = container.query(Car.class).get(0);
            final Pilot thePilot = container.query(Pilot.class).get(0);
            Pilot pilotViaCar = theCar.getPilot();
            assertTrue(thePilot == pilotViaCar);
            // #end example
        } finally {
            container.close();
        }
    }

    private static void storeAndLoadWithTheSame() {
        ObjectContainer rootContainer = Db4oEmbedded.openFile(DATABASE_FILE);
        ObjectContainer container1 = rootContainer.ext().openSession();
        ObjectContainer container2 = rootContainer.ext().openSession();
        try {
            // #example: Loading with different object container results in different objects
            final Car loadedWithContainer1 = container1.query(Car.class).get(0);
            final Car loadedWithContainer2 = container2.query(Car.class).get(0);
            assertFalse(loadedWithContainer1 == loadedWithContainer2);
            // #end example
        } finally {
            container1.close();
            container2.close();
            rootContainer.close();
        }
    }

    private static void storeOnDifferentContainers() {
        ObjectContainer rootContainer = Db4oEmbedded.openFile(DATABASE_FILE);
        ObjectContainer container1 = rootContainer.ext().openSession();
        ObjectContainer container2 = rootContainer.ext().openSession();
        try {
            // #example: Don't use different object-container for the same object.
            final Car loadedWithContainer1 = container1.query(Car.class).get(0);
            container2.store(loadedWithContainer1);
            // Now the car is store twice.
            // Because the container2 cannot recognize objects from other containers
            // Therefore always use the same container to store and load objects
            printAll(container2.query(Car.class));
            // #end example
        } finally {
            container1.close();
            container2.close();
            rootContainer.close();
        }
    }

    private static void removeFromReferenceCache() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: With purge you can remove objects from the reference cache
            final Car theCar = container.query(Car.class).get(0);
            container.ext().purge(theCar);
            // #end example
        } finally {
            container.close();
        }

    }

    private static void printAll(ObjectSet<?> objects) {
        for (Object object : objects) {
            System.out.println(object);
        }
    }

    private static void assertTrue(boolean mustBeTrue) {
        if (!mustBeTrue) {
            throw new AssertionError("expected true");
        }
    }

    private static void assertFalse(boolean mustBeTrue) {
        if (mustBeTrue) {
            throw new AssertionError("expected false");
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }

    private static void storeAObject() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            container.store(new Car(new Pilot("John"), "VW Golf"));
        } finally {
            container.close();
        }
    }
}
