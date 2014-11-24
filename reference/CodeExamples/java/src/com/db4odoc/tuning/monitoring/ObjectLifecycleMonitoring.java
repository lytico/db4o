package com.db4odoc.tuning.monitoring;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.monitoring.ObjectLifecycleMonitoringSupport;

import java.io.IOException;
import java.util.Random;


public class ObjectLifecycleMonitoring {
    public static void main(String[] args) {

        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Monitor the object lifecycle statistics
        configuration.common().add(new ObjectLifecycleMonitoringSupport());
        // #end example:
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        try {
            System.out.println("Press any key to end application...");
            workWithObjects(container);
            System.out.println("done.");
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            container.close();
        }
    }
    private static void workWithObjects(ObjectContainer container) throws IOException {
        while(System.in.available()==0){
            Random rnd = new Random();
            storeData(container, rnd);
            deleteData(container,rnd);
            container.commit();
        }
    }

    private static void deleteData(ObjectContainer container, Random rnd) {
        final ObjectSet<DataObject> data = container.query(DataObject.class);
        for(int i=0;i<rnd.nextInt(4096);i++){
            final DataObject obj = data.get(rnd.nextInt(data.size()));
            if(null!=obj){
                container.delete(obj);
            }
        }

    }

    private static void storeData(ObjectContainer container, Random rnd) {
        for(int i=0;i<rnd.nextInt(4096);i++){
            container.store(new DataObject(rnd));
        }
    }
}
