/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.ext;

/**
 * db4o-specific exception. <br><br>
 * This exception is thrown when the object container required for 
 * the current operation was closed or failed to open.
 * @see com.db4o.Db4o#openFile
 * @see com.db4o.ObjectContainer#close
 */
public class DatabaseClosedException extends Db4oFatalException {

}
