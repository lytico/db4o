/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.ext;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;

/**
 * extended functionality for the ObjectServer interface.
 * <br><br>Every ObjectServer also always is an ExtObjectServer
 * so a cast is possible.<br><br>
 * {@link com.db4o.ObjectServer#ext}
 * is a convenient method to perform the cast.<br><br>
 * The functionality is split to two interfaces to allow newcomers to
 * focus on the essential methods.
 */
public interface ExtObjectServer extends ObjectServer{
    
    /**
     * backs up the database file used by the ObjectServer.
     * <br><br>While the backup is running, the ObjectServer can continue to be
     * used. Changes that are made while the backup is in progress, will be applied to
     * the open ObjectServer and to the backup.<br><br>
     * While the backup is running, the ObjectContainer should not be closed.<br><br>
     * If a file already exists at the specified path, it will be overwritten.<br><br>
     * @param path a fully qualified path
     */
    public void backup(String path) throws IOException;
    
    /**
     * returns the number of connected clients.
     */
    public int clientCount();
	
	 /**
	 * returns the {@link Configuration} context for this ObjectServer.
	 * <br><br>
	 * @return the Configuration context for this ObjectServer
     */
	public Configuration configure();
	
	/**
	 * returns the ObjectContainer used by the server.
	 * <br><br>
	 * @return the ObjectContainer used by the server
	 */
	public ObjectContainer objectContainer();
	
	/**
	 * removes client access permissions for the specified user.
	 * <br><br>
	 * @param userName the name of the user
	 */
	public void revokeAccess(String userName);
	
	/**
	 * @return The local port this server uses, 0 if disconnected or in embedded mode
	 */
    public int port();
}
