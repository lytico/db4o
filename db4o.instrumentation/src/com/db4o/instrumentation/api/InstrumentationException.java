/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.api;

import com.db4o.foundation.*;

public class InstrumentationException extends ChainedRuntimeException {

	public InstrumentationException(Throwable cause) {
		super(cause.getMessage(), cause);
	}
	
}
