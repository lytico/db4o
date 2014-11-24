/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.ext;

import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * db4o exception wrapper: Exceptions occurring during internal processing
 * will be proliferated to the client calling code encapsulated in an exception
 * of this type. The original exception, if any, is available through
 * Db4oException#getCause().
 */
public class Db4oException extends ChainedRuntimeException {
	
	/**
	 * Simple constructor
	 */
	public Db4oException() {
		this(null, null);
	}
	
	/**
	 * Constructor with an exception message specified 
	 * @param msg exception message 
	 */
	public Db4oException(String msg) {
		this(msg, null);
	}

	/**
	 * Constructor with an exception cause specified
	 * @param cause exception cause
	 */
	public Db4oException(Throwable cause) {
		this(null, cause);
	}
	
	/**
	 * Constructor with an exception message selected
	 * from the internal message collection. 
	 * @param messageConstant internal db4o message number
	 */
	public Db4oException(int messageConstant){
		this(Messages.get(messageConstant));
	}
	
	/**
	 * Constructor with an exception message and cause specified
	 * @param msg exception message
	 * @param cause exception cause
	 */
	public Db4oException(String msg, Throwable cause) {
		super(msg, cause);
	}
}
