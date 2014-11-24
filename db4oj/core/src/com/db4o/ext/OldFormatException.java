/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.ext;

import com.db4o.internal.*;



/**
 * db4o-specific exception.<br><br>
 * This exception is thrown when an old file format was detected 
 * and {@link com.db4o.config.Configuration#allowVersionUpdates(boolean)}
 * is set to false.
 */
public class OldFormatException extends Db4oFatalException {
	
	/**
	 * Constructor with the default message. 
	 */
	public OldFormatException() {
		super(Messages.OLD_DATABASE_FORMAT);
	}
}
