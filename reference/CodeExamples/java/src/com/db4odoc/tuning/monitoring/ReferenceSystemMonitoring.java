package com.db4odoc.tuning.monitoring;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.monitoring.ReferenceSystemMonitoringSupport;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;


public class ReferenceSystemMonitoring {
    public static void main(String[] args) {

        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Add reference system monitoring
        configuration.common().add(new ReferenceSystemMonitoringSupport());
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        try {
            storeObjects(container);
            System.out.println("Press any key to end application...");
            blowReferenceSystem(container);
            System.out.println("done.");
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            container.close();
        }
    }

    private static void storeObjects(ObjectContainer container) {
        Random rnd= new Random();
        for(int i=0;i<500000;i++){
            container.store(new DataObject(rnd));
        }
        container.commit();
    }

    private static void blowReferenceSystem(ObjectContainer container) throws IOException {
        final ObjectSet<DataObject> dataObjects = container.query(DataObject.class);
        List<DataObject> hardReferences = new ArrayList<DataObject>();
        while(System.in.available()==0){
            for (DataObject reference : dataObjects) {
                hardReferences.add(reference);
            }
            hardReferences.clear();
        }
    }
}
