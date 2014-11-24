package com.db4odoc.typehandling.translator;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;


public class TranslatorExample {
    public static void main(String[] args) {

        ObjectContainer container = createDB();
        try {
            // #example: Store the non storable type
            container.store(new NonStorableType("TestData"));
            // #end example
        } finally {
            container.close();
        }

        container = createDB();
        try {
            // #example: Load the non storable type
            NonStorableType instance = container.query(NonStorableType.class).get(0);
            // #end example
            System.out.println(instance.getData());
        } finally {
            container.close();
        }
    }

    private static ObjectContainer createDB() {
        // #example: Register type translator for the NonStorableType-class
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(NonStorableType.class).translate(new ExampleTranslator());
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        // #end example
        return container; 
    }
}
