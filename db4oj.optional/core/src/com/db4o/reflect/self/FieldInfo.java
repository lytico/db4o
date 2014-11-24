/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect.self;

public class FieldInfo {
	private String _name;
	private Class _clazz;
	private boolean _isPublic; 
	private boolean _isStatic; 
	private boolean _isTransient;

	
	
	public FieldInfo(String name, Class clazz, boolean isPublic, boolean isStatic, boolean isTransient) {
		_name = name;
		_clazz = clazz;
		_isPublic = isPublic;
		_isStatic = isStatic;
		_isTransient = isTransient;
	}

	public String name() {
		return _name;
	}
	
	public Class type() {
		return _clazz;
	}

	public boolean isPublic() {
		return _isPublic;
	}

	public boolean isStatic() {
		return _isStatic;
	}

	public boolean isTransient() {
		return _isTransient;
	}
}
