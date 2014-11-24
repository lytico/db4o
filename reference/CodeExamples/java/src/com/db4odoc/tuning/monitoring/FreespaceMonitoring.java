package com.db4odoc.tuning.monitoring;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.monitoring.FreespaceMonitoringSupport;

import java.io.IOException;
import java.util.Random;


public class FreespaceMonitoring {

    public static void main(String[] args) {

        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Monitor the free-space system
        configuration.common().add(new FreespaceMonitoringSupport());
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        try {
            System.out.println("Press any key to end application...");
            tryToFragmentDatabase(container);
            System.out.println("done.");
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            container.close();
        }
    }

    private static void tryToFragmentDatabase(ObjectContainer container) throws IOException {
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
