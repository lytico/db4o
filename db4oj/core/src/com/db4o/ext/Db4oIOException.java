/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.ext;

/**
 * db4o-specific exception.<br><br>
 * This exception is thrown when a system IO exception
 * is encounted by db4o process.
  */
public class Db4oIOException extends Db4oFatalException {

	/**
	 * Constructor.
	 */
	public Db4oIOException() {
		super();
	}
	
	public Db4oIOException(String message) {
		super(message);
	}

	/**
	 * Constructor allowing to specify the causing exception
	 * @param cause exception cause 
	 */
	public Db4oIOException(Throwable cause) {
		super(cause.getMessage(), cause);
	}
}
