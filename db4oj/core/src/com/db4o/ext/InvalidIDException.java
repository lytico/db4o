/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.ext;

/**
 * db4o-specific exception.<br><br>
 * This exception is thrown when the supplied object ID
 * is incorrect (outside the scope of the database IDs).
 * @see com.db4o.ext.ExtObjectContainer#bind(Object, long)
 * @see com.db4o.ext.ExtObjectContainer#getByID(long)
 */
public class InvalidIDException extends Db4oRecoverableException {
	
	/**
	 * Constructor allowing to specify the exception cause
	 * @param cause cause exception
	 */
	public InvalidIDException(Throwable cause) {
		super(cause);
	}
	
	/**
	 * Constructor allowing to specify the offending id 
	 * @param id the offending id
	 */
	public InvalidIDException(int id){
		super("id: " + id);
	}
}
