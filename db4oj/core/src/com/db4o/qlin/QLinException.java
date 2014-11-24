/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.qlin;

import com.db4o.ext.*;

/**
 * exceptions to signal improper use of the
 * {@link QLin} query interface.  
 */
public class QLinException extends Db4oException{
	
	public QLinException(String message) {
		super(message);
	}

	public QLinException(Throwable cause) {
		super(cause);
	}

}
