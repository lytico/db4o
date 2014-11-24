package com.db4odoc.deletion;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;

import java.io.File;


public class DeletionExamples {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {

        simpleDeletion();
        referenceIsSetToNull();
        cascadeDeletion();

        removeFromCollection();
        removeAndDelete();
    }

    private static void simpleDeletion() {
        prepareDBWithCarAndPilot();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Deleting object is as simple as storing
            Car car = findCar(container);
            container.delete(car);
            // We've deleted the only care there is
            assertEquals(0,allCars(container).size());
            // The pilots are still there
            assertEquals(1,allPilots(container).size());
            // #end example
        } finally {
            container.close();
        }
    }

    private static void referenceIsSetToNull() {
        prepareDBWithCarAndPilot();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Delete the pilot
            Pilot pilot = findPilot(container);
            container.delete(pilot);
            // #end example
        } finally {
            container.close();
        }
        container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Reference is null after deleting
            // Now the car's reference to the car is set to null
            Car car = findCar(container);
            assertEquals(null,car.getPilot());
            // #end example
        } finally {
            container.close();
        }

    }

    private static void cascadeDeletion() {
        prepareDBWithCarAndPilot();
        // #example: Mark field for cascading deletion
        EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        config.common().objectClass(Car.class).objectField("pilot").cascadeOnDelete(true);
        ObjectContainer container = Db4oEmbedded.openFile(config,DATABASE_FILE);
        // #end example
        try {
            // #example: Cascade deletion
            Car car = findCar(container);
            container.delete(car);
            // Now the pilot is also gone
            assertEquals(0,allPilots(container).size());
            // #end example
        } finally {
            container.close();
        }
    }

    private static void removeFromCollection() {
        prepareDBWithPilotGroup();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Removing from a collection doesn't delete the collection-members
            PilotGroup group = findGroup(container);
            final Pilot pilot = group.getPilots().get(0);
            group.getPilots().remove(pilot);
            container.store(group.getPilots());

            assertEquals(3,allPilots(container).size());
            assertEquals(2,group.getPilots().size());
            // #end example
        } finally {
            container.close();
        }
    }

    private static void removeAndDelete() {
        prepareDBWithPilotGroup();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Remove and delete
            PilotGroup group = findGroup(container);
            final Pilot pilot = group.getPilots().get(0);
            group.getPilots().remove(pilot);
            container.store(group.getPilots());
            container.delete(pilot);

            assertEquals(2,allPilots(container).size());
            assertEquals(2,group.getPilots().size());
            // #end example
        } finally {
            container.close();
        }
    }

    void mm(){

        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().outStream(System.out);
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        try {

        } finally {
            container.close();
        }
    }

    private static void assertEquals(Object expectedValue, Object actualValue) {
        if(!nullSafeEquals(expectedValue, actualValue)){
            throw new RuntimeException("Expected "+expectedValue+" but got "+actualValue);
        }
    }

    private static boolean nullSafeEquals(Object expectedValue, Object actualValue) {
        return (expectedValue == null && actualValue == null)
                || (expectedValue != null && expectedValue.equals(actualValue));
    }

    private static Pilot findPilot(ObjectContainer container) {
        return allPilots(container).get(0);
    }

    private static ObjectSet<Pilot> allPilots(ObjectContainer container) {
        return container.query(Pilot.class);
    }
    private static Car findCar(ObjectContainer container) {
        return allCars(container).get(0);
    }
    private static ObjectSet<Car> allCars(ObjectContainer container) {
        return container.query(Car.class);
    }
    private static PilotGroup findGroup(ObjectContainer container) {
        return container.query(PilotGroup.class).get(0);
    }

    private static void prepareDBWithCarAndPilot() {
        cleanUp();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            container.store(new Car(new Pilot("John"), "VM Golf"));
        } finally {
            container.close();
        }
    }
    private static void prepareDBWithPilotGroup() {
        cleanUp();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            container.store(new PilotGroup(
                    new Pilot("John"),
                    new Pilot("Jenny"),
                    new Pilot("Joanna")
            ));
        } finally {
            container.close();
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }

}
