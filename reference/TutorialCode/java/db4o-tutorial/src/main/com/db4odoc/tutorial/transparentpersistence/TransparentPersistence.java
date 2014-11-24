package com.db4odoc.tutorial.transparentpersistence;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.ta.Activatable;
import com.db4o.ta.DeactivatingRollbackStrategy;
import com.db4o.ta.TransparentPersistenceSupport;

public class TransparentPersistence {

    private static final String DATABASE_FILE_NAME = "database.db4o";

    public static void main(String[] args) {
        checkEnhancement();
        storeExampleObjects();
        activationJustWorks();
        updatesJustWork();
    }

    private static void activationJustWorks() {
        // #example: Configure transparent persistence
        final EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().add(new TransparentPersistenceSupport(new DeactivatingRollbackStrategy()));
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE_NAME);
        // #end example
        try {
            //#example: Transparent persistence manages activation
            Driver driver = queryForDriver(container);
            // Transparent persistence will activate objects as needed
            System.out.println("Is activated? "+container.ext().isActive(driver));
            String nameOfDriver = driver.getName();
            System.out.println("The name is "+nameOfDriver);
            System.out.println("Is activated? "+container.ext().isActive(driver));
            //#end example
        } finally {
            container.close();
        }
    }

    private static void updatesJustWork() {
        ObjectContainer container = openDatabase();
        try {
            // #example: Just update and commit. Transparent persistence manages all updates
            Driver driver = queryForDriver(container);
            driver.getMostLovedCar().setCarName("New name");
            driver.setName("John Turbo");
            driver.addOwnedCar(new Car("Volvo Combi"));
            // Just commit the transaction. All modified objects are stored
            container.commit();
            // #end example
        } finally {
            container.close();
        }
    }

    private static void checkEnhancement() {
        // #example: Check for enhancement
        if (!Activatable.class.isAssignableFrom(Car.class)) {
            throw new AssertionError("Expect that the " + Car.class + " implements " + Activatable.class);
        }
        // #end example
        if (!Activatable.class.isAssignableFrom(Driver.class)) {
            throw new AssertionError("Expect that the " + Driver.class + " implements " + Activatable.class);
        }
    }

    private static void storeExampleObjects() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE_NAME);
        try {
            Car beetle = new Car("VW Beetle");
            Car ferrari = new Car("Ferrari");

            Driver driver = new Driver("John", ferrari);
            driver.addOwnedCar(beetle);
            driver.addOwnedCar(ferrari);

            container.store(driver);
        } finally {
            container.close();
        }
    }

    private static ObjectContainer openDatabase() {
        final EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().add(new TransparentPersistenceSupport(new DeactivatingRollbackStrategy()));
        return Db4oEmbedded.openFile(configuration, DATABASE_FILE_NAME);
    }

    private static Driver queryForDriver(ObjectContainer container) {
        return container.query(Driver.class).get(0);
    }
}
