package com.db4odoc.tutorial.updating;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;

    public class UpdateConcept {
        private static final String DATABASE_FILE = "database.db4o";

        public static void main(String[] args) {
            storeExampleObjects();
            updatingDriverDoesNotUpdateCar();
            dealWithUpdateDepth();
            increaseUpdateDepth();
        }

        private static void storeExampleObjects() {
            ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
            try {
                // #example: Store a driver and his cars
                Car beetle = new Car("VW Beetle");
                Car ferrari = new Car("Ferrari");

                Driver driver = new Driver("John", ferrari);
                driver.addOwnedCar(beetle);
                driver.addOwnedCar(ferrari);

                container.store(driver);
                // #end example:
            } finally {
                container.close();
            }
        }

        private static void updatingDriverDoesNotUpdateCar() {
            ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
            try {
                // #example: Update the driver and his cars
                Driver driver = queryForDriver(container);
                driver.setName("Johannes");
                driver.getMostLovedCar().setCarName("Red Ferrari");
                driver.addOwnedCar(new Car("Fiat Punto"));
                container.store(driver);
                // #end example
            } finally {
                container.close();
            }
            printOutContent();
        }

        private static void increaseUpdateDepth() {
            // #example: Increase the update depth to 2
            EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
            configuration.common().updateDepth(2);
            // #end example

            ObjectContainer container = Db4oEmbedded.openFile(configuration,DATABASE_FILE);
            try {
                Driver driver = queryForDriver(container);
                driver.setName("Joe");
                driver.getMostLovedCar().setCarName("Red Ferrari");
                driver.addOwnedCar(new Car("Fiat Punto"));
                container.store(driver);
            } finally {
                container.close();
            }
            printOutContent();
        }

        private static void dealWithUpdateDepth() {
            ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
            try {
                Driver driver = queryForDriver(container);
                driver.setName("Joe");
                driver.getMostLovedCar().setCarName("Red Ferrari");
                driver.addOwnedCar(new Car("Fiat Punto"));
                // #example: Update everything explicitly
                container.store(driver);
                container.store(driver.getMostLovedCar());
                container.store(driver.getOwnedCars());
                // #end example
            } finally {
                container.close();
            }
            printOutContent();
        }

        private static void printOutContent() {
            ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
            try {
                // #example: Check the updated objects
                Driver driver = queryForDriver(container);
                // Is updated
                System.out.println(driver.getName());
                // Isn't updated at all
                System.out.println(driver.getMostLovedCar().getCarName());
                // Also the owned car list isn't updated
                for (Car car : driver.getOwnedCars()) {
                    System.out.println(car);
                }
                // #end example

            } finally {
                container.close();
            }
        }

        private static EmbeddedConfiguration moreUpdateOptions() {
            // #example: More update options
            EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
            // Update all referenced objects for the Driver class
            configuration.common().objectClass(Driver.class).cascadeOnUpdate(true);
            // #end example
            return configuration;
        }

        private static Driver queryForDriver(ObjectContainer container) {
            return container.query(Driver.class).get(0);
        }
    }
