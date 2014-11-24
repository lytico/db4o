package com.db4odoc.pitfalls.typehandling;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;

import java.io.File;


public class TypeHandlingEdgeCases {
    public static void main(String[] args) {
        cleanUp();
        sqlDateCannotBeStored();
        collectionsAreTreatedSpecially();
    }

    private static void collectionsAreTreatedSpecially() {
        storeCollections();
        readCollections();
    }

    private static void readCollections() {
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            // #example: db4o fails to load partially implemented collections
            try {
                ObjectSet<CollectionHolder> holders = container.query(CollectionHolder.class);
                MyFixedSizeCollection<String> collection = holders.get(0).getNames();
            } catch (Exception e) {
                // this will fail! The db4o collection-storage
                // assumes that collections support all operations of the collection interface.
                // db4o uses the regular collection-methods to restore the instance.
                e.printStackTrace();
            }
            // #end example
        } finally {
            container.close();
        }
    }

    private static void storeCollections() {        
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            container.store(new CollectionHolder("Hi","there","how","are","you"));
        } finally {
            container.close();
        }
    }

    private static void sqlDateCannotBeStored() {
        storeDates();
        readDates();
    }

    private static void readDates() {
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            final ObjectSet<DatesHolder> queries = container.query(DatesHolder.class);
            for (DatesHolder query : queries) {
                System.out.println("Regular Date:"+query.getRegularDate());
                System.out.println("SQL Date:"+query.getSqlField());
            }
        } finally {
            container.close();
        }
    }

    private static void storeDates() {
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            container.store(new DatesHolder());
        } finally {
            container.close();
        }
    }

    private static void cleanUp() {
        new File("database.db4o").delete();
    }

}



