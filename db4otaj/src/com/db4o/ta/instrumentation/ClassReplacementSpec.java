/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.ta.instrumentation;


public class ClassReplacementSpec {

	public final Class _origClass;
	public final Class _replacementClass;

	public ClassReplacementSpec(Class origType, Class replacementType) {
		_origClass = origType;
		_replacementClass = replacementType;
	}
}
