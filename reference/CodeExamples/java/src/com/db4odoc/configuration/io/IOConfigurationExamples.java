package com.db4odoc.configuration.io;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.io.*;


public class IOConfigurationExamples {

    public static void fileStorage(){
        // #example: Using the pure file storage
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        Storage fileStorage = new FileStorage();
        configuration.file().storage(fileStorage);
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        // #end example

    }
    
    public static void cachingStorage(){
        // #example: Using a caching storage
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        Storage fileStorage = new FileStorage();
        // A cache with 128 pages of 1024KB size, gives a 128KB cache
        Storage cachingStorage = new CachingStorage(fileStorage,128,1024);
        configuration.file().storage(cachingStorage);
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        // #end example

    }

    public static void nonFlushingStorage(){
        // #example: Using the non-flushing storage
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        Storage fileStorage = new FileStorage();
        // the non-flushing storage improves performance, but risks database corruption.
        Storage cachingStorage = new NonFlushingStorage(fileStorage);
        configuration.file().storage(cachingStorage);
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        // #end example

    }

    public static void specifyGrowStrategyForMemoryStorage(){
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Using memory-storage with constant grow strategy
        GrowthStrategy growStrategy = new ConstantGrowthStrategy(100);
        MemoryStorage memory = new MemoryStorage(growStrategy);
        configuration.file().storage(memory);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");

    }

    public static void usingMemoryStorage(){
        // #example: Using memory-storage
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        MemoryStorage memory = new MemoryStorage();
        configuration.file().storage(memory);
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        // #end example

    }

    public static void usingPagingMemoryStorage(){
        // #example: Using paging memory-storage
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        PagingMemoryStorage memory = new PagingMemoryStorage();
        configuration.file().storage(memory);
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        // #end example
        container.close();

    }

    public static void storageStack(){
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: You stack up different storage-decorator to add functionality
        // the basic file storage
        Storage fileStorage = new FileStorage();
        // add your own decorator
        Storage myStorageDecorator = new MyStorageDecorator(fileStorage);
        // add caching to the storage
        Storage storageWithCaching = new CachingStorage(myStorageDecorator);
        // finally configure db4o with our storage-stack
        configuration.file().storage(storageWithCaching);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        
    }


    /**
     * This decorator does nothing. It's just used as an example
     */
    private static class MyStorageDecorator extends StorageDecorator{

        public MyStorageDecorator(Storage storage) {
            super(storage);
        }
    }
}
