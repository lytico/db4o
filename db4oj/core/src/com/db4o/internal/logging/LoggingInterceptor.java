package com.db4o.internal.logging;


public interface LoggingInterceptor {

	public void log(Level loggingLevel, String method, Object[] args);

}
