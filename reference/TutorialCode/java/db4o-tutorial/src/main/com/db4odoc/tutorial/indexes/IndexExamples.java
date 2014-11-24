package com.db4odoc.tutorial.indexes;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;

public class IndexExamples {

    private static void configureIndexes() {
        // #example: Configure index externally
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(Car.class).objectField("carName").indexed(true);

        ObjectContainer container = Db4oEmbedded.openFile(configuration,"database.db4o");
        // #end example
    }
}
