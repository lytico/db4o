package com.db4odoc.acidproperties;

import com.db4o.Db4oEmbedded;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.io.FileStorage;
import com.db4o.io.NonFlushingStorage;
import com.db4o.io.Storage;


public class DangerousPractises {


    public  static void dangerousStorage(){
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Using the non-flushing storage weakens the ACID-properties
        Storage fileStorage = new FileStorage();
        configuration.file().storage(new NonFlushingStorage(fileStorage));
        // #end example
        Db4oEmbedded.openFile(configuration,"database.db4o");
    }
    public  static void dangerousNonRecovering(){
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Disabling commit-recovery weakens the ACID-properties
        configuration.file().disableCommitRecovery();
        // #end example
        Db4oEmbedded.openFile(configuration,"database.db4o");
    }
}
