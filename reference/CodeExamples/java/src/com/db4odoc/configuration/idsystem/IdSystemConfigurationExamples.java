package com.db4odoc.configuration.idsystem;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;


public class IdSystemConfigurationExamples {

    private static void stackedBTreeIdSystem(){
        // #example: Use stacked B-trees for storing the ids
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.idSystem().useStackedBTreeSystem();
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration,"database.db4o");
        container.close();
    }
    private static void bTreeIdSystem(){
        // #example: Use a single B-tree for storing the ids.
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.idSystem().useSingleBTreeSystem();
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration,"database.db4o");
        container.close();
    }  
    private static void useMemoryIDSystem(){
        // #example: Use a in-memory id system
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.idSystem().useInMemorySystem();
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration,"database.db4o");
        container.close();
    }

    private static void pointerIdSystem(){
        // #example: Use pointers for storing the ids
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.idSystem().usePointerBasedSystem();
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration,"database.db4o");
        container.close();
    }
    private static void customIdSystem(){
        // #example: use a custom id system
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.idSystem().useCustomSystem(new CustomIdSystemFactory());
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration,"database.db4o");
        container.close();
    }
}
    
