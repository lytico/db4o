/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.ext;

import com.db4o.foundation.*;

/**
 * Unexpected fatal error is encountered.
 */
public class Db4oUnexpectedException extends ChainedRuntimeException {

	public Db4oUnexpectedException(Throwable cause) {
		super(cause.getMessage(), cause);
	}

}
