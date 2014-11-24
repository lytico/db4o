package com.db4odoc.query.soda;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Query;
import com.db4o.query.QueryComparator;

import java.io.File;


public class SodaSorting {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUp();
        final EmbeddedConfiguration cfg = Db4oEmbedded.newConfiguration();
        ObjectContainer container = Db4oEmbedded.openFile(cfg, DATABASE_FILE);
        try {
            storeData(container);

            sortingOnField(container);
            sortingOnMultipleFields(container);
            customOrder(container);
        } finally {
            container.close();
        }
    }

    private static void sortingOnField(ObjectContainer container) {
        System.out.println("Order by a field");
        // #example: Order by a field
        final Query query = container.query();
        query.constrain(Pilot.class);
        query.descend("name").orderAscending();

        final ObjectSet<Object> result = query.execute();
        // #end example
        listResult(result);
    }

    private static void sortingOnMultipleFields(ObjectContainer container) {
        System.out.println("Order by multiple fields");
        // #example: Order by multiple fields
        final Query query = container.query();
        query.constrain(Pilot.class);
        // order first by age, then by name
        query.descend("age").orderAscending();
        query.descend("name").orderAscending();

        final ObjectSet<Object> result = query.execute();
        // #end example
        listResult(result);
    }

    private static void customOrder(ObjectContainer container) {
        System.out.println("Order by your comparator");
        // #example: Order by your comparator
        Query query = container.query();
        query.constrain(Pilot.class);
        query.sortBy(new QueryComparator<Pilot>() {
            public int compare(Pilot o, Pilot o1) {
                // sort by string-length
                return (int)Math.signum(o.getName().length() - o1.getName().length());
            }
        });

        final ObjectSet<Object> result = query.execute();
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
        container.store(new Pilot("John", 42));
        container.store(new Pilot("Joanna", 45));
        container.store(new Pilot("Brigit", 59));
        container.store(new Pilot("Jenny", 21));
        container.store(new Pilot("Rick", 33));
        container.store(new Pilot("Jolanda", 33));
        container.store(new Pilot("Chris", 22));
        container.store(new Pilot("John", 33));
        container.store(new Pilot("Raphael", 34));
        container.store(new Pilot("Paul", 61));
        container.store(new Pilot("Li", 43));

    }
}
