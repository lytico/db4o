/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.internal;

public final class CallBackMode {
	public final static CallBackMode ALL = new CallBackMode("ALL");
	public final static CallBackMode DELETE_ONLY = new CallBackMode("DELETE_ONLY");
	public final static CallBackMode NONE = new CallBackMode("NONE");

	private String _desc;
	
	private CallBackMode(String desc) {
		_desc = desc;
	}
	
	@Override
	public String toString() {
		return _desc;
	}
}