package com.db4odoc.tuning.monitoring;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.monitoring.NativeQueryMonitoringSupport;
import com.db4o.monitoring.QueryMonitoringSupport;
import com.db4o.query.Predicate;

import java.io.IOException;
import java.util.Random;


public class QueryMonitoring {
    public static void main(String[] args) {

        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Add query monitoring
        configuration.common().add(new QueryMonitoringSupport());
        configuration.common().add(new NativeQueryMonitoringSupport());
        // #end example
        configuration.common().objectClass(DataObject.class).objectField("indexedNumber").indexed(true);
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        try {
            storeALotOfObjects(container);
            System.out.println("Press any key to end application...");
            queryLoop(container);
            System.out.println("done.");
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            container.close();
        }
    }

    private static void queryLoop(ObjectContainer container) throws IOException {
        while(System.in.available()==0){
            runUnoptimizedQuery(container);
            runQueryOnNotIndexedField(container);
            runQueryOnIndexedField(container);
        }
    }

    private static void runQueryOnIndexedField(ObjectContainer container) {
        final ObjectSet<DataObject> result = container.query(new Predicate<DataObject>() {
            @Override
            public boolean match(DataObject o) {
                return o.getIndexedNumber() == 42;
            }
        });
    }

    private static void runQueryOnNotIndexedField(ObjectContainer container) {
        final ObjectSet<DataObject> result = container.query(new Predicate<DataObject>() {
            @Override
            public boolean match(DataObject o) {
                return o.getNumber() == 42;
            }
        });
    }

    private static void runUnoptimizedQuery(ObjectContainer container) {
        final ObjectSet<DataObject> result = container.query(new Predicate<DataObject>() {
            @Override
            public boolean match(DataObject o) {
                return o.getNumber() == new Random().nextInt();
            }
        });
    }

    private static void storeALotOfObjects(ObjectContainer container) {
        Random rnd = new Random();
        for(int i=0;i<10000;i++){
            container.store(new DataObject(rnd.nextInt()));
        }
    }

    private static class DataObject{
        private int number;
        private int indexedNumber;

        private DataObject(int number) {
            this.number = number;
            this.indexedNumber = number;
        }

        public int getNumber() {
            return number;
        }

        public int getIndexedNumber() {
            return indexedNumber;
        }
    }
}
