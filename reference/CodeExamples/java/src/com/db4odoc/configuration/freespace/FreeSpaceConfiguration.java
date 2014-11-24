package com.db4odoc.configuration.freespace;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;


public class FreeSpaceConfiguration {

    public static void discardSettings(){
        // #example: Discard settings
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // discard smaller than 256 bytes
        configuration.file().freespace().discardSmallerThan(256);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();

    }
    public static void useBTreeSystem(){
        // #example: Use BTree system
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().freespace().useBTreeSystem();
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();
    }
    public static void useInMemorySystem(){
        // #example: Use the in memory system
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().freespace().useRamSystem();
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();
    }
    public static void freespaceFiller(){
        // #example: Using a freespace filler
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().freespace().freespaceFiller(new MyFreeSpaceFiller());
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();
    }
}
