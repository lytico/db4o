package com.db4odoc.callbacks.examples;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.events.*;
import com.db4o.query.Predicate;

import java.io.File;


public class CallbackExamples {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUp();


        storeTestObjects();
        referentialIntegrity();
    }

    private static void referentialIntegrity() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        // #example: Referential integrity
        final EventRegistry events = EventRegistryFactory.forObjectContainer(container);
        events.deleting().addListener(new EventListener4<CancellableObjectEventArgs>() {
            @Override
            public void onEvent(Event4<CancellableObjectEventArgs> events,
                                CancellableObjectEventArgs eventArgs) {
                final Object toDelete = eventArgs.object();
                if(toDelete instanceof Pilot){
                    final ObjectContainer container = eventArgs.objectContainer();
                    final ObjectSet<Car> cars = container.query(new Predicate<Car>() {
                        @Override
                        public boolean match(Car car) {
                            return car.getPilot() == toDelete;
                        }
                    });
                    if(cars.size()>0){
                        eventArgs.cancel();
                    }
                }
            }
        });
        // #end example
        try {
            Pilot pilot = container.query(Pilot.class).get(0);
            container.delete(pilot);
        } finally {
            container.close();
        }
    }

    private static void storeTestObjects() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            Pilot pilot = new Pilot("John");
            container.store(pilot);
            container.store(new Car(pilot));
        } finally {
            container.close();
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }
}
