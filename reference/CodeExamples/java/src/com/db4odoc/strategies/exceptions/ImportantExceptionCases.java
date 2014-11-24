package com.db4odoc.strategies.exceptions;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.constraints.UniqueFieldValueConstraint;
import com.db4o.constraints.UniqueFieldValueConstraintViolationException;
import com.db4o.cs.Db4oClientServer;
import com.db4o.ext.DatabaseFileLockedException;
import com.db4o.ext.Db4oIOException;


public class ImportantExceptionCases {
    public static void main(String[] args) {
        alreadyOpenDatabaseThrows();
        connectToNotExistingServer();
        uniqueViolation();
    }

    private static void uniqueViolation() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(UniqueId.class).objectField("id").indexed(true);
        configuration.common().add(new UniqueFieldValueConstraint(UniqueId.class,"id"));
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        try {
            // #example: Violation of the unique constraint
            container.store(new UniqueId(42));
            container.store(new UniqueId(42));
            try{
                container.commit();
            } catch (UniqueFieldValueConstraintViolationException e){
                // Violated the unique-constraint!
                // Retry with a new value or handle this gracefully
                container.rollback();
            }
            // #end example
        } finally {
            container.close();
        }
    }

    private static void connectToNotExistingServer() {
        // #example: Cannot connect to the server
        try{
            final ObjectContainer container = Db4oClientServer.openClient("localhost", 1337, "sa", "sa");

        } catch(Db4oIOException e){
            // Couldn't connect to the server.
            // Ask for new connection-settings or handle this case gracefully
        }
        // #end example
    }

    private static void alreadyOpenDatabaseThrows() {
        ObjectContainer allReadyOpen = Db4oEmbedded.openFile("database.db4o");
        try {
            // #example: If the database is already open
            try{
                ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
            } catch (DatabaseFileLockedException e){
                // Database is already open!
                // Use another database-file or handle this case gracefully
            }
            // #end example
        } finally {
            allReadyOpen.close();
        }
    }


    private static class UniqueId{
        private int id;

        private UniqueId(int id) {
            this.id = id;
        }
    }
}
