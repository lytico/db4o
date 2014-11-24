package com.db4odoc.strategies.storingstatic;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;

public class StoringStaticFields {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        storeCars();
        loadCars();

    }

    private static void loadCars() {
        ObjectContainer container = openDatabase();
        try {
            final ObjectSet<Car> cars = container.query(Car.class);

            for (Car car : cars) {
                // #example: Compare by reference
                // When you enable persist static field values, you can compare by reference
                // because db4o stores the static field
                if(car.getColor()== Color.BLACK){
                    System.out.println("Black cars are boring");
                } else if(car.getColor()== Color.RED){
                    System.out.println("Fire engine?");
                }
                // #end example

            }
        } finally {
            container.close();
        }

    }

    private static void storeCars() {
        ObjectContainer container = openDatabase();
        try {
            container.store(new Car(Color.BLACK));
            container.store(new Car(Color.WHITE));
            container.store(new Car(Color.GREEN));
            container.store(new Car(Color.RED));
        } finally {
            container.close();
        }

    }

    private static ObjectContainer openDatabase() {
        //#example: Enable storing static fields for our color class
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Color.class).persistStaticFieldValues();
        // #end example
        return Db4oEmbedded.openFile(configuration, DATABASE_FILE);
    }
}
