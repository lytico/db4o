package com.db4odoc.pitfalls.updatedepth;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Predicate;

import java.io.File;

public class UpdateDepthPitfall {
    public static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUpAndPrepare();

        toLowUpdateDepthOnObject();
        toLowUpdateDepthOnCollection();

        explicitlyUpdateObjects();
        explicitlyStateUpdateDepth();
        updateDepthForCollection();
    }

    private static void toLowUpdateDepthOnObject() {
        // #example: Update depth limits what is store when updating objects
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            Car car = queryForCar(container);
            car.setCarName("New Mercedes");
            car.getDriver().setName("New Driver Name");

            // With the default-update depth of one, only the changes
            // on the car-object are stored, but not the changes on
            // the person
            container.store(car);
        } finally {
            container.close();
        }
        container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            Car car = queryForCar(container);
            System.out.println("Car-Name:"+car.getCarName());
            System.out.println("Driver-Name:"+car.getDriver().getName());
        } finally {
            container.close();
        }
        // #end example
    }
    private static void toLowUpdateDepthOnCollection() {
        // #example: Update doesn't work on collection
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            Person jodie = queryForJodie(container);
            jodie.add(new Person("Jamie"));
            // Remember that a collection is also a regular object
            // so with the default-update depth of one, only the changes
            // on the person-object are stored, but not the changes on
            // the friend-list.
            container.store(jodie);
        } finally {
            container.close();
        }
        container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            Person jodie = queryForJodie(container);
            for (Person person : jodie.getFriends()) {
                // the added friend is gone, because the update-depth is to low
                System.out.println("Friend="+person.getName());
            }
        } finally {
            container.close();
        }
        // #end example
    }

    private static void explicitlyUpdateObjects() {
        cleanUp();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Explicitly store changes on the driver
            Car car = queryForCar(container);
            car.setCarName("New Mercedes");
            car.getDriver().setName("New Driver Name");

            // Explicitly store the driver to ensure that those changes are also in the database
            container.store(car);
            container.store(car.getDriver());
            // #end example
        } finally {
            container.close();
        }
        printCar();
    }
    private static void explicitlyStateUpdateDepth() {
        cleanUp();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Explicitly use the update depth
            Car car = queryForCar(container);
            car.setCarName("New Mercedes");
            car.getDriver().setName("New Driver Name");

            // Explicitly state the update depth
            container.ext().store(car,2);
            // #end example
        } finally {
            container.close();
        }
        printCar();
    }

    private static void updateDepthForCollection() {
        // #example: A higher update depth fixes the issue
        EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        config.common().updateDepth(2);
        ObjectContainer container = Db4oEmbedded.openFile(config,DATABASE_FILE);
        // #end example
        try {
            Person jodie = queryForJodie(container);
            jodie.add(new Person("Jamie"));
            container.store(jodie);
        } finally {
            container.close();
        }
        config = Db4oEmbedded.newConfiguration();
        config.common().updateDepth(2);
        container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            Person jodie = queryForJodie(container);
            for (Person person : jodie.getFriends()) {
                // the added friend is gone, because the update-depth is to low
                System.out.println("Friend="+person.getName());
            }
        } finally {
            container.close();
        }
    }

    private static void printCar() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            Car car = queryForCar(container);
            System.out.println("Car-Name:"+car.getCarName());
            System.out.println("Driver-Name:"+car.getDriver().getName());
        } finally {
            container.close();
        }
    }


    private static Car queryForCar(ObjectContainer container) {
        return container.query(Car.class).get(0);
    }

    private static void cleanUpAndPrepare() {
        cleanUp();
        prepareDeepObjGraph();
    }


    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }

    private static Person queryForJodie(ObjectContainer container) {
        return container.query(new Predicate<Person>() {
            @Override
            public boolean match(Person o) {
                return o.getName().equals("Jodie");
            }
        }).get(0);
    }

    private static void prepareDeepObjGraph() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            Person jodie = new Person("Jodie");

            jodie.add(new Person("Joanna"));
            jodie.add(new Person("Julia"));

            container.store(jodie);

            container.store(new Car(new Person("Janette"),"Mercedes" ));
        } finally {
            container.close();
        }
    }

}
