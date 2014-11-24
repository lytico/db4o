package com.db4o.ext;

import com.db4o.*;

/**
 * db4o exception wrapper: Exceptions occurring during internal processing
 * will be proliferated to the client calling code encapsulated in an exception
 * of rhis type. The original exception, if any, is available through
 * {@link Db4oException#cause()}.
 */
public class Db4oException extends RuntimeException {
	private Exception _cause;
	
	public Db4oException(String msg) {
		super(msg);
	}

	public Db4oException(Exception cause) {
		this(cause.toString());
		_cause = cause;
	}
	
	public Db4oException(int messageConstant){
		this(Messages.get(messageConstant));
	}

	/**
	 * @return The originating exception, if any
	 */
	public Exception cause() {
		return _cause;
	}
}
