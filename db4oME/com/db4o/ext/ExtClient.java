/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.ext;

/**
 * extended client functionality for the
 * {@link ExtObjectContainer ExtObjectContainer} interface.
 * <br><br>Both 
 * {@link com.db4o.Db4o#openClient Db4o.openClient()} methods always
 * return an <code>ExtClient</code> object so a cast is possible.<br><br>
 * The ObjectContainer functionality is split into multiple interfaces to allow newcomers to
 * focus on the essential methods.
 */
public interface ExtClient extends ExtObjectContainer{

	/**
	 * requests opening a different server database file for this client session.
	 * <br><br>
	 * This method can be used to switch between database files from the client
	 * side while not having to open a new socket connection or closing the
	 * current one.
	 * <br><br>
	 * If the database file does not exist on the server, it will be created.
	 * <br><br>
	 * A typical usecase:<br>
	 * The main database file is used for login, user and rights management only.
	 * Only one single db4o server session needs to be run. Multiple satellite
	 * database files are used for different applications or multiple user circles.
	 * Storing the data to multiple database files has the following advantages:<br>
	 * - easier rights management<br>
	 * - easier backup<br>
	 * - possible later load balancing to multiple servers<br>
	 * - better performance of smaller individual database files<br>
	 * - special debugging database files can be used
	 * <br><br>
	 * User authorization to the alternative database file will not be checked.
	 * <br><br>
	 * All persistent references to objects that are currently in memory
	 * are discarded during the switching process.<br><br>
	 * @param fileName the fully qualified path of the requested database file.
	 */
	public void switchToFile(String fileName);
	
	
	/**
	 * requests switching back to the main database file after a previous call
	 * to <code>switchToFile(String fileName)</code>.
	 * <br><br>
	 * All persistent references to objects that are currently in memory
	 * are discarded during the switching process.<br><br>
	 */
	public void switchToMainFile();
	
	public boolean isAlive();
}

