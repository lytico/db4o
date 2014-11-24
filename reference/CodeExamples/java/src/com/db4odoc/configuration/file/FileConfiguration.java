package com.db4odoc.configuration.file;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.ConfigScope;
import com.db4o.config.EmbeddedConfiguration;

import java.io.IOException;


public class FileConfiguration {

    public static void asynchronousSync(){
        // #example: Allow asynchronous synchronisation of the file-flushes
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().asynchronousSync(true);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();

    }

    public static void changeBlobPath(){
        // #example: Configure the blob-path
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        try {
            configuration.file().blobPath("myBlobDirectory");
        } catch (IOException e) {
            e.printStackTrace();
        }
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();

    }

    public static void increaseBlockSize(){
        // #example: Increase block size for larger databases
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().blockSize(8);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();
    }

    public static void globalUUID(){
        // #example: Enable db4o uuids globally
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().generateUUIDs(ConfigScope.GLOBALLY);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();
    }
    public static void individualUUID(){
        // #example: Enable db4o uuids for certain classes
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().generateUUIDs(ConfigScope.INDIVIDUALLY);
        configuration.common().objectClass(SpecialClass.class).generateUUIDs(true);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");

        SpecialClass withUUID = new SpecialClass();
        container.store(withUUID);
        NormalClass withoutUUID = new NormalClass();
        container.store(withoutUUID);

        assertNotNull(container.ext().getObjectInfo(withUUID).getUUID());
        assertNull(container.ext().getObjectInfo(withoutUUID).getUUID());

        container.close();
    }

    public static void commitTimestamps(){
        // #example: Enable db4o commit timestamps
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().generateCommitTimestamps(true);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();
    }

    public static void reserveSpace(){
        // #example: Configure the growth size
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().databaseGrowthSize(4096);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();

    }
    public static void disableCommitRecovers(){
        // #example: Disable commit recovery
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().disableCommitRecovery();
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();

    }

    public static void doNotLockDatabaseFile(){
        // #example: Disable the database file lock
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().lockDatabaseFile(false);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();
    }

    public static void readOnlyMode(){
        // #example: Set read only mode
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().readOnly(true);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();

    }
    public static void recoveryMode(){
        // #example: Enable recovery mode to open a corrupted database
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().recoveryMode(true);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();
    }
    public static void reserveStorageSpace(){
        // #example: Reserve storage space
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().reserveStorageSpace(1024*1024);
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        container.close();
    }

    private static void assertNotNull(Object uuid) {
        if(null==uuid){
            throw new RuntimeException("Expected not null");
        }
    }
    private static void assertNull(Object uuid) {
        if(null!=uuid){
            throw new RuntimeException("Expected null");
        }
    }
    private static void assertTrue(boolean value) {
        if(!value){
            throw new RuntimeException("Expected true");
        }
    }


    private static class SpecialClass {
        
    }
    private static class NormalClass {

    }
}
