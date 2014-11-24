/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.connection;

@SuppressWarnings("serial")
public class DBConnectException extends Exception {

	private final ConnectionParams params;
	
	public DBConnectException(ConnectionParams params, String message, Throwable cause) {
		super(message, cause);
		this.params = params;		
	}

	public DBConnectException(String message, Throwable cause) {
		this(null, message, cause);
	}

	public DBConnectException(ConnectionParams params, String message) {
		this(params, message, null);
	}

	public DBConnectException(String message) {
		this(null, message, null);
	}

	public DBConnectException(ConnectionParams params, Throwable cause) {
		this(params, cause.getMessage(), cause);
		
	}

	public ConnectionParams params() {
		return params;
	}
	
}
