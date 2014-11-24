package com.db4odoc.deframentation;

import com.db4o.Db4oEmbedded;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.defragment.*;
import com.db4o.io.FileStorage;

import java.io.IOException;


public class DefragmentationConfigurationExamples {

    public void configureFile() throws IOException {
        // #example: Configure the file
        DefragmentConfig config = new DefragmentConfig("database.db4o");

        Defragment.defrag(config);
        // #end example
    }

    public void configureBackupFile() throws IOException {
        // #example: Configure the file and backup file
        DefragmentConfig config = new DefragmentConfig("database.db4o", "database.db4o.back");

        Defragment.defrag(config);
        // #end example
    }

    public void setMappingImplementation() throws IOException {
        // #example: Choose a id mapping system
        IdMapping mapping = new InMemoryIdMapping();
        DefragmentConfig config = new DefragmentConfig("database.db4o", "database.db4o.back", mapping);

        Defragment.defrag(config);
        // #end example
    }

    public void setDb4oConfiguration() throws IOException {
        // #example: Use the database-configuration
        DefragmentConfig config = new DefragmentConfig("database.db4o");
        // It's best to use the very same configuration you use for the regular database
        final EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        config.db4oConfig(configuration);

        Defragment.defrag(config);
        // #end example
    }

    public void setCommitFrequency() throws IOException {
        // #example: Set the commit frequency
        DefragmentConfig config = new DefragmentConfig("database.db4o");
        config.objectCommitFrequency(10000);

        Defragment.defrag(config);
        // #end example
    }

    public void changeBackupStorage() throws IOException {
        // #example: Use a separate storage for the backup
        DefragmentConfig config = new DefragmentConfig("database.db4o");
        config.backupStorage(new FileStorage());

        Defragment.defrag(config);
        // #end example
    }

    public void deleteBackup() throws IOException {
        // #example: Delete the backup after the defragmentation process
        DefragmentConfig config = new DefragmentConfig("database.db4o");
        config.forceBackupDelete(true);

        Defragment.defrag(config);
        // #end example
    }

    public void disableReadOnlyForBackup() throws IOException {
        // #example: Disable readonly on backup
        DefragmentConfig config = new DefragmentConfig("database.db4o");
        config.readOnly(false);

        Defragment.defrag(config);
        // #end example
    }

    public void useAClassFilter() throws IOException {
        // #example: Use class filter
        DefragmentConfig config = new DefragmentConfig("database.db4o");
        config.storedClassFilter(new AvailableClassFilter());

        Defragment.defrag(config);
        // #end example
    }

    public void upgradeDb4oFile() throws IOException {
        // #example: Upgrade database version
        DefragmentConfig config = new DefragmentConfig("database.db4o");
        config.upgradeFile(System.getProperty("java.io.tmpdir"));

        Defragment.defrag(config);
        // #end example
    }
}
