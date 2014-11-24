package com.db4odoc.disconnectedobj.merging;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Predicate;

import java.io.File;
import java.util.UUID;


public class MergeExample {
    private static final String DATABASE_FILE_NAME = "database.db4o";


    public static void main(String[] args) {
        mergeExample();
    }

    private static void mergeExample() {
        cleanUp();

        storeCar();
        printCars();

        Car car = getCarByName("Slow Car");
        updateCar(car);

        updateWithMerging(car);


        printCars();

        cleanUp();
    }

    private static void updateWithMerging(Car disconnectedCar) {
        // #example: merging
        ObjectContainer container = openDatabase();

        // first get the object from the database
        Car carInDb = getCarById(container,disconnectedCar.getObjectId());

        // copy the value-objects (int, long, double, string etc)
        carInDb.setName(disconnectedCar.getName());

        // traverse into the references
        Pilot pilotInDB = carInDb.getPilot();
        Pilot disconnectedPilot = disconnectedCar.getPilot();

        // check if the object is still the same
        if(pilotInDB.getObjectId().equals(disconnectedPilot.getObjectId())){
            // if it is, copy the value-objects
            pilotInDB.setName(disconnectedPilot.getName());
            pilotInDB.setPoints(disconnectedPilot.getPoints());
        } else{
            // otherwise replace the object
            carInDb.setPilot(disconnectedPilot); 
        }

        // finally store the changes
        container.store(pilotInDB);
        container.store(carInDb);
        // #end example
        container.close();

    }

    private static void updateCar(Car car) {
        car.setName("Fast Car");
        car.getPilot().setPoints(300);
    }

    private static void printCars() {
        ObjectContainer container = openDatabase();
        ObjectSet<Car> cars = container.query(Car.class);
        for (Car car : cars) {
            System.out.println(car);
        }
        container.close();
    }

    private static Car getCarById(ObjectContainer container,final UUID id) {
        return container.query(new Predicate<Car>() {
            @Override
            public boolean match(Car car) {
                return car.getObjectId().equals(id);
            }
        }).get(0);
    }

    private static Car getCarByName(final String carName) {
        ObjectContainer container = openDatabase();
        Car result = container.query(new Predicate<Car>() {
            @Override
            public boolean match(Car car) {
                return car.getName().equals(carName);
            }
        }).get(0);
        container.close();
        return result;
    }


    private static void storeCar() {
        ObjectContainer container = openDatabase();
        container.store(new Car(new Pilot("Joe",200),"Slow Car"));
        container.close();
    }


    private static void cleanUp() {
        new File(DATABASE_FILE_NAME).delete();
    }


    private static ObjectContainer openDatabase() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(IDHolder.class).objectField("uuid").indexed(true);
        return Db4oEmbedded.openFile(configuration,DATABASE_FILE_NAME);
    }
}
