package com.db4odoc.configuration.objectconfig;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.config.TSerializable;


public class ObjectConfigurationExamples {
    private static final String DATABASE_FILE = "database.db4o";


    private static void callConstructor() {
        // #example: Call constructor
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).callConstructor(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void cascadeOnActivate() {
        // #example: When activated, activate also all references
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).cascadeOnActivate(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void cascadeOnDelete() {
        // #example: When deleted, delete also all references
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).cascadeOnDelete(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void cascadeOnUpdate() {
        // #example: When updated, update also all references
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).cascadeOnUpdate(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void generateUUIDs() {
        // #example: Generate uuids for this type
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).generateUUIDs(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void indexObjects() {
        // #example: Disable class index
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).indexed(false);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void maximumActivationDepth() {
        // #example: Set maximum activation depth
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).maximumActivationDepth(5);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void minimalActivationDepth() {
        // #example: Set minimum activation depth
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).minimumActivationDepth(2);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void persistStaticFieldValues() {
        // #example: Persist also the static fields
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).persistStaticFieldValues();
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void rename() {
        // #example: Rename this type
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).rename("com.db4odoc.new.package.NewName");
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void storeTransientFields() {
        // #example: Store also transient fields
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).storeTransientFields(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void translator() {
        // #example: Use a translator for this type
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).translate(new TSerializable());
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }


    private static void updateDepth() {
        // #example: Set the update depth
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).updateDepth(2);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }
}
