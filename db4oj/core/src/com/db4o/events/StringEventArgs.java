/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.events;

/**
 * @since 7.12
 */
public class StringEventArgs extends EventArgs {
	
	public StringEventArgs(String message) {
		_message = message;
	}
	
	/**
	 * @sharpen.property
	 */
	public String message() { 
		return _message;
	}
	
	private final String _message;
	
}
