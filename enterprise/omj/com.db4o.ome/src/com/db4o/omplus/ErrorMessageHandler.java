/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus;

public class ErrorMessageHandler {
	private final ErrorMessageSink sink;
	
	public ErrorMessageHandler(ErrorMessageSink sink) {
		this.sink = sink;
	}

	public void error(String msg)  {
		error(msg, null);
	}

	public void error(Throwable exc) {
		error(exc.getMessage(), exc);
	}
	
	public void error(String msg, Throwable exc) {
		sink.showError(msg);
		if(exc != null) {
			sink.showExc(msg, exc);	
		}
	}
}