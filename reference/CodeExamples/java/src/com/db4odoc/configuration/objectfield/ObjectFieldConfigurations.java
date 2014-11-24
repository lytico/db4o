package com.db4odoc.configuration.objectfield;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;


public class ObjectFieldConfigurations {
    private static final String DATABASE_FILE = "database.db4o";

    private static void indexField() {
        // #example: Index a certain field
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).objectField("name").indexed(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }
    private static void cascadeOnActivate() {
        // #example: When activated, activate also the object referenced by this field
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).objectField("father").cascadeOnActivate(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }
    private static void cascadeOnUpdate() {
        // #example: When updated, update also the object referenced by this field
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).objectField("father").cascadeOnUpdate(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }
    private static void cascadeOnDelete() {
        // #example: When deleted, delete also the object referenced by this field
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).objectField("father").cascadeOnDelete(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }
    private static void renameField() {
        // #example: Rename this field
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Person.class).objectField("name").rename("sirname");
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }
}

