/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
package com.db4o.internal.threading;

import com.db4o.events.*;

public class UncaughtExceptionEventArgs extends EventArgs {

	private Throwable _exception;

	public UncaughtExceptionEventArgs(Throwable e) {
		_exception = e;
    }

	/**
	 * @sharpen.property
	 */
	public Throwable exception() {
		return _exception;
    }

}
