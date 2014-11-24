/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.io.*;

/**
 * Represents a local ObjectContainer attached to a database file.
 * @since 7.10
 */
public interface EmbeddedObjectContainer extends ObjectContainer{
	
	
    /**
     * Backs up a database file of an open ObjectContainer.
     * <br><br>
     * While the backup is running, the ObjectContainer can continue to be
     * used. Changes that are made while the backup is in progress will be applied to
     * the object container and to the backup.<br><br>
     * While the backup is running, the object container should not be closed.<br><br>
     * If a file already exists at the specified path, it will be overwritten.<br><br>
     * The {@link Storage} used for backup is the one configured for this container. If you
     * want to use another storage implementation for the backup please
     * use {@link #ext() ext()}.{@link ExtObjectContainer#backup(com.db4o.io.Storage, String) backup(com.db4o.io.Storage, String)}.
     * @param path a the path to the backup file
     * @throws DatabaseClosedException db4o database file was closed or failed to open.
     * @throws NotSupportedException is thrown when the operation is not supported in current 
     * configuration/environment
     * @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
     */
    public void backup(String path) throws Db4oIOException,
			DatabaseClosedException, NotSupportedException;

}
