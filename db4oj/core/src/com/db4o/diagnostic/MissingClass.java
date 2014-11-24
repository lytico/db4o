/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;

/**
 * Diagnostic if class not found
 */
public class MissingClass extends DiagnosticBase{
	
	public final String _className;
	
	public MissingClass(String className) {
		_className = className;
	}

	@Override
	public String problem() {
		return "Class not found in classpath.";
	}

	@Override
	public Object reason() {
		return _className;
	}

	@Override
	public String solution() {
		return "Check your classpath.";
	}

}
