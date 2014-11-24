/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.ext;

/**
 * db4o-specific exception.<br><br>
 * This exception is thrown when the database file reaches the
 * maximum allowed size. Upon throwing the exception the database is
 * switched to the read-only mode. <br>
 * The maximum database size is configurable 
 * and can reach up to 254GB.
 *@see com.db4o.config.Configuration#blockSize(int)
 */
public class DatabaseMaximumSizeReachedException extends Db4oRecoverableException {

	public DatabaseMaximumSizeReachedException(int size) {
		super("Maximum database file size reached. Last valid size: " + size + ". From now on opening only works in read-only mode.");
	}

}
