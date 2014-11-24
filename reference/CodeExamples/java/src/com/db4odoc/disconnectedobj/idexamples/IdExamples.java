package com.db4odoc.disconnectedobj.idexamples;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Predicate;

import java.io.File;

public class IdExamples<TId> {
    private static final String DATABASE_FILE_NAME = "database.db4o";
    private final IdExample<TId> toRun;

    public IdExamples(IdExample<TId> toRun) {
        this.toRun = toRun;
    }

    public static void main(String[] args) {
        runExamples(
                createRunner(Db4oInternalIdExample.create()),
                createRunner(Db4oUuidExample.create()),
                createRunner(UuidOnObject.create()),
                createRunner(AutoIncrementExample.create())
        );

    }

    private static <TId> IdExamples<TId> createRunner(IdExample<TId> toRun) {
        return new IdExamples<TId>(toRun);
    }

    private static void runExamples(IdExamples<?>... examplesToRun) {
        for (IdExamples<?> toRun : examplesToRun) {
            toRun.run();
        }
    }

    private void run() {
        System.out.println("Running: " + toRun.getClass().getSimpleName());
        cleanUp();
        storeJoe();

        TId id = idOfJoe();
        Pilot incomingChanges = new Pilot("Joe Junior");

        updateJoe(id, incomingChanges);

        assertWasUpdated();
        listAllPilots();

        cleanUp();

    }

    private void assertWasUpdated() {
        ObjectContainer container = openDatabase();
        ObjectSet<Pilot> pilots = container.query(Pilot.class);
        assertEquals(1, pilots.size());
        assertEquals("Joe Junior", pilots.get(0).getName());
        container.close();

    }

    private void assertEquals(Object expected, Object actual) {
        if (!expected.equals(actual)) {
            throw new AssertionError("Expected to be " + expected + " but is " + actual);
        }
    }

    private void listAllPilots() {
        ObjectContainer container = openDatabase();
        ObjectSet<Pilot> pilots = container.query(Pilot.class);
        for (Pilot pilot : pilots) {
            System.out.println(pilot);
        }
        container.close();
    }


    private void updateJoe(TId id, Pilot incomingChanges) {
        ObjectContainer container = openDatabase();
        Pilot joe = (Pilot) toRun.objectForID(id, container);
        mergeChanges(joe, incomingChanges);
        container.store(joe);
        container.close();
    }

    private void mergeChanges(Pilot toUpdate, Pilot incomingChanges) {
        toUpdate.setName(incomingChanges.getName());
    }

    private TId idOfJoe() {
        ObjectContainer container = openDatabase();
        Pilot joe = queryByName(container, "Joe");
        TId id = toRun.idForObject(joe, container);
        container.close();
        return id;
    }


    private Pilot queryByName(ObjectContainer container, final String name) {
        return container.query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot p) {
                return p.getName().equals(name);
            }
        }).get(0);
    }

    private void storeJoe() {
        ObjectContainer container = openDatabase();
        Pilot joe = new Pilot("Joe");
        container.store(joe);
        container.close();
    }


    private static void cleanUp() {
        new File(DATABASE_FILE_NAME).delete();
    }


    private ObjectContainer openDatabase() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        toRun.configure(configuration);
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE_NAME);
        toRun.registerEventOnContainer(container);
        return container;
    }


}
