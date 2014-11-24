package com.db4odoc.tp.enhancement;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Predicate;
import com.db4o.ta.Activatable;
import com.db4o.ta.DeactivatingRollbackStrategy;
import com.db4o.ta.TransparentActivationSupport;
import com.db4o.ta.TransparentPersistenceSupport;

public class TransparentPersistence {
    private static final String DATABASE_FILE_NAME = "database.db4o";

    public static void main(String[] args) {
        checkEnhancement();
        storeExampleObjects();
        activationJustWorks();
        updatingJustWorks();
    }


    private static void checkEnhancement() {
        // #example: Check for enhancement
        if (!Activatable.class.isAssignableFrom(Person.class)) {
            throw new AssertionError("Expect that the " + Person.class + " implements " + Activatable.class);
        }
        // #end example
    }


    private static void storeExampleObjects() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE_NAME);
        try {
            Person person = Person.personWithHistory();

            container.store(person);
        } finally {
            container.close();
        }
    }

    public static void activationJustWorks() {
        ObjectContainer container = openDatabaseWithTA();
        try {
            // #example: Activation just works
            Person person = queryByName(container, "Joanna the 10");
            Person beginOfDynasty = person.getMother();

            // With transparent activation enabled, you can navigate deeply
            // nested object graphs. db4o will ensure that the objects
            // are loaded from the database.
            while (null != beginOfDynasty.getMother()) {
                beginOfDynasty = beginOfDynasty.getMother();
            }
            System.out.println(beginOfDynasty.getName());
            // #end example
        } finally {
            container.close();
        }
    }

    private static void updatingJustWorks() {
        ObjectContainer container = openDatabaseWithTP();
        try {
            // #example: Just update and commit. Transparent persistence manages all updates
            Person person = queryByName(container, "Joanna the 10");
            Person mother = person.getMother();
            mother.setName("New Name");
            // Just commit the transaction. All modified objects are stored
            container.commit();
            // #end example
        } finally {
            container.close();
        }
    }

    private static Person queryByName(ObjectContainer container, final String name) {
        return container.query(new Predicate<Person>() {
            @Override
            public boolean match(Person o) {
                return o.getName().equals(name);
            }
        }).get(0);
    }

    private static ObjectContainer openDatabaseWithTA() {
        // #example: Add transparent activation
        final EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().add(new TransparentActivationSupport());
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE_NAME);
        // #end example
        return container;
    }

    private static ObjectContainer openDatabaseWithTP() {
        // #example: Add transparent persistence
        final EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().add(new TransparentPersistenceSupport(new DeactivatingRollbackStrategy()));
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE_NAME);
        // #end example
        return container;
    }
}
