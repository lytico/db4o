/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.ext;

import java.io.*;

import com.db4o.foundation.*;

/**
 *  @sharpen.partial 
 */
public class CompositeDb4oException extends ChainedRuntimeException {

	public final Throwable[] _exceptions;
	
	public CompositeDb4oException(Throwable... exceptions) {
		_exceptions = exceptions;
	}
	
	/**
	 * @sharpen.ignore
	 */
	@Override
	public void printStackTrace(PrintStream s) {
		super.printStackTrace(s);
		for (Throwable t : _exceptions) {
			s.println("contains");
			t.printStackTrace(s);
		}
	}
	
	/**
	 * @sharpen.ignore
	 */
	@Override
	public void printStackTrace(PrintWriter s) {
		super.printStackTrace(s);
		for (Throwable t : _exceptions) {
			s.println("contains");
			t.printStackTrace(s);
		}
	}
	/**
	 * Overriding for JDK 1.1 compatibility.
	 * @sharpen.ignore
	 */
	@Override
    public void printStackTrace() {
		synchronized(System.err){
			printStackTrace(System.err);
		}
    }
	
}
