package com.db4odoc.tuning.monitoring;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.monitoring.IOMonitoringSupport;

import java.io.IOException;
import java.util.Random;


public class IOMonitoring {
    public static void main(String[] args) {

        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Add IO-Monitoring
        configuration.common().add(new IOMonitoringSupport());
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        try {
            System.out.println("Press any key to end application...");
            doIOOperations(container);
            System.out.println("done.");
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            container.close();
        }
    }

    private static void doIOOperations(ObjectContainer container) throws IOException {
        while(System.in.available()==0){
            storeALot(container);
            readALot(container);
        }
    }

    private static void readALot(ObjectContainer container) {
        final ObjectSet<DataObject> allObjects = container.query(DataObject.class);
        for (DataObject object : allObjects) {
            object.toString();
        }
    }

    private static void storeALot(ObjectContainer container) {
        Random rnd = new Random();
        for(int i=0;i<1024;i++){
            container.store(new DataObject(rnd));
        }
        container.commit();
    }

}
