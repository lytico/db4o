/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;

import com.db4o.config.*;

/**
 * Diagnostic if {@link ObjectClass#objectField(String)} was called on a 
 * field that does not exist.
 */
public class ObjectFieldDoesNotExist extends DiagnosticBase{
	
	public final String _className;
	
	public final String _fieldName;

	public ObjectFieldDoesNotExist(String className, String fieldName) {
		_className = className;
		_fieldName = fieldName;
	}

	@Override
	public String problem() {
		return "ObjectField was configured but does not exist.";
	}

	@Override
	public Object reason() {
		return _className + "." + _fieldName;
	}

	@Override
	public String solution() {
		return "Check your configuration.";
	}

}
