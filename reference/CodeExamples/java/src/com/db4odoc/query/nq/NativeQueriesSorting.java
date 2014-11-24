package com.db4odoc.query.nq;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Predicate;
import com.db4o.query.QueryComparator;

import java.io.File;


public class NativeQueriesSorting {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUp();
        final EmbeddedConfiguration cfg = Db4oEmbedded.newConfiguration();
        ObjectContainer container = Db4oEmbedded.openFile(cfg,DATABASE_FILE);
        try {
            storeData(container);

            nativeQuerySorting(container);
        } finally {
            container.close();
        }
    }

    private static void nativeQuerySorting(ObjectContainer container) {
        // #example: Native query with sorting
        final ObjectSet<Pilot> pilots = container.query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot o) {
                return o.getAge() > 18;
            }
        }, new QueryComparator<Pilot>() {
            public int compare(Pilot pilot, Pilot pilot1) {
                return pilot.getName().compareTo(pilot1.getName());
            }
        });
        // #end example

        listResult(pilots);
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
