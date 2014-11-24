package com.db4odoc.configuration.alias;

import com.db4o.Db4oEmbedded;
import com.db4o.EmbeddedObjectContainer;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.config.TypeAlias;
import com.db4o.config.WildcardAlias;

import java.io.File;


public class AliasExamples {
    private static final String DATABASE_FILE_NAME = "database.db4o";
    public static void main(String[] args) {
        cleanUp();

        aliasesExample();
    }

    private static void aliasesExample() {
        storeTypes();

        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Adding aliases
        // add an alias for a specific type
        configuration.common().addAlias(
                new TypeAlias("com.db4odoc.configuration.alias.OldTypeInDatabase",
                        "com.db4odoc.configuration.alias.NewType"));
        // or add an alias for a whole namespace
        configuration.common().addAlias(
                new WildcardAlias("com.db4odoc.configuration.alias.old.location.*",
                        "com.db4odoc.configuration.alias.current.location.*"));
        // #end example

        EmbeddedObjectContainer container = Db4oEmbedded.openFile(configuration,DATABASE_FILE_NAME);
        try{
            int countRenamed = container.query(NewType.class).size();
            assertFoundEntries(countRenamed);
            int countInOtherPackage = container
                    .query(com.db4odoc.configuration.alias.current.location.Car.class).size();
            assertFoundEntries(countInOtherPackage);
        } finally {
            container.close();
        }


    }

    private static void assertFoundEntries(int countRenamed) {
        if(1>countRenamed){
            throw new RuntimeException("Expected a least on entry");    
        }
    }

    private static void storeTypes() {
        ObjectContainer database = openDatabase();
        try{
            database.store(new OldTypeInDatabase());
            database.store(new com.db4odoc.configuration.alias.old.location.Car());
        }finally {
            database.close();   
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE_NAME).delete();
    }

    private static ObjectContainer openDatabase() {
        return Db4oEmbedded.openFile(DATABASE_FILE_NAME);
    }
}

class OldTypeInDatabase {
    private String name = "default";

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}

class NewType {
    private String name = "default";

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
