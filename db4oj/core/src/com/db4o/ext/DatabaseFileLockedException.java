/* Copyright (C) 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.ext;


/**
 * db4o-specific exception.<br><br>
 * this Exception is thrown during any of the db4o open calls
 * if the database file is locked by another process.
 * @see com.db4o.Db4o#openFile
 */
public class DatabaseFileLockedException extends Db4oFatalException {
	
	/**
	 * Constructor with a database description message 
	 * @param databaseDescription message, which can help to identify the database
	 */
	public DatabaseFileLockedException(String databaseDescription) {
		super(databaseDescription);
	}

	/**
	 * Constructor with a database description and cause exception
	 * @param databaseDescription database description
	 * @param cause previous exception caused DatabaseFileLockedException
	 */
	public DatabaseFileLockedException(String databaseDescription, Throwable cause) {
		super(databaseDescription,cause);
	}
	
}
