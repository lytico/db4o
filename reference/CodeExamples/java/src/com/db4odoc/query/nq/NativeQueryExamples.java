package com.db4odoc.query.nq;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Predicate;

import java.io.File;
import java.util.Arrays;
import java.util.List;


public class NativeQueryExamples {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUp();
        EmbeddedConfiguration cfg = Db4oEmbedded.newConfiguration();
        ObjectContainer container = Db4oEmbedded.openFile(cfg,DATABASE_FILE);
        try {
            storeData(container);

            equality(container);
            comparison(container);
            rageOfValues(container);
            combineComparisons(container);
            followReferences(container);
            queryInSeparateClass(container);
            anyCode(container);
        } finally {
            container.close();
        }
    }

    private static void equality(ObjectContainer container) {
        // #example: Check for equality of the name
        ObjectSet<Pilot> result = container.query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot pilot) {
                return pilot.getName().equals("John");
            }
        });
        // #end example

        listResult(result);
    }

    private static void comparison(ObjectContainer container) {
        // #example: Compare values to each other
        ObjectSet<Pilot> result = container.query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot pilot) {
                return pilot.getAge() > 18;
            }
        });
        // #end example

        listResult(result);
    }

    private static void rageOfValues(ObjectContainer container) {
        // #example: Query for a particular rage of values
        ObjectSet<Pilot> result = container.query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot pilot) {
                return pilot.getAge() > 18 && pilot.getAge()<30;
            }
        });
        // #end example

        listResult(result);
    }

    private static void combineComparisons(ObjectContainer container) {
        // #example: Combine different comparisons with the logical operators
        ObjectSet<Pilot> result = container.query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot pilot) {
                return (pilot.getAge() > 18 && pilot.getAge()<30)
                        || pilot.getName().equals("John");
            }
        });
        // #end example

        listResult(result);
    }

    private static void followReferences(ObjectContainer container) {
        // #example: You can follow references
        ObjectSet<Car> result = container.query(new Predicate<Car>() {
            @Override
            public boolean match(Car car) {
                return car.getPilot().getName().equals("John");
            }
        });
        // #end example

        listResult(result);
    }

    private static void queryInSeparateClass(ObjectContainer container) {
        // #example: Use the predefined query
        ObjectSet<Pilot> result = container.query(new AllJohns());
        // #end example

        listResult(result);
    }

    private static void anyCode(ObjectContainer container) {
        // #example: Arbitrary code
        final List<Integer> allowedAges = Arrays.asList(18,20,33,55);
        ObjectSet<Pilot> result = container.query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot pilot) {
                return allowedAges.contains(pilot.getAge()) ||
                       pilot.getName().toLowerCase().equals("John"); 
            }
        });
        // #end example

        listResult(result);
    }


    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }


    private static void listResult(ObjectSet result) {
        for (Object object : result) {
            System.out.println(object);
        }
    }

    private static void storeData(ObjectContainer container) {
        Pilot john = new Pilot("John",42);
        Pilot joanna = new Pilot("Joanna",45);
        Pilot jenny = new Pilot("Jenny",21);
        Pilot rick = new Pilot("Rick",33);
        Pilot juliette = new Pilot("Juliette",33);

        container.store(new Car(john,"Ferrari"));
        container.store(new Car(joanna,"Mercedes"));
        container.store(new Car(jenny,"Volvo"));
        container.store(new Car(rick,"Fiat"));
        container.store(new Car(juliette,"Suzuki"));

    }
}
