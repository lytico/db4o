/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.ext;

public class Db4oFatalException extends Db4oException {

	public Db4oFatalException() {
		super();
		
	}

	public Db4oFatalException(int messageConstant) {
		super(messageConstant);
		
	}

	public Db4oFatalException(String msg, Throwable cause) {
		super(msg, cause);
		
	}

	public Db4oFatalException(String msg) {
		super(msg);
		
	}

	public Db4oFatalException(Throwable cause) {
		super(cause);
		
	}

}
