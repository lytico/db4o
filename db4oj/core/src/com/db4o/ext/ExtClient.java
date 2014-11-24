/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.ext;


/**
 * extended client functionality for the
 * {@link ExtObjectContainer ExtObjectContainer} interface.
 * <br><br>Both 
 * {@link com.db4o.Db4oClientServer#openClient Db4oClientServer.openClient()} methods always
 * return an ExtClient object so a cast is possible.<br><br>
 * The ObjectContainer functionality is split into multiple interfaces to allow newcomers to
 * focus on the essential methods.
 */
public interface ExtClient extends ExtObjectContainer{
	
    /**
     * checks if the client is currently connected to a server.
     * @return true if the client is alive.
     */
	public boolean isAlive();
	
}

