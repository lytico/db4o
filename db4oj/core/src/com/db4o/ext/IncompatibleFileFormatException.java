/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.ext;

/**
 * db4o-specific exception.<br><br>
 * This exception is thrown when the database file format
 * is not compatible with the applied configuration.
 */
public class IncompatibleFileFormatException extends Db4oFatalException {

	public IncompatibleFileFormatException(){
		super();
	}
	
	public IncompatibleFileFormatException(String message) {
		super(message);
	}

}
