/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.ext;

public class Db4oRecoverableException extends Db4oException {

	public Db4oRecoverableException() {
		super();
		
	}

	public Db4oRecoverableException(int messageConstant) {
		super(messageConstant);
		
	}

	public Db4oRecoverableException(String msg, Throwable cause) {
		super(msg, cause);
		
	}

	public Db4oRecoverableException(String msg) {
		super(msg);
		
	}

	public Db4oRecoverableException(Throwable cause) {
		super(cause);
		
	}

}
