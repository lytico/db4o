/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal;

public class ClassInfo {
	
	public static ClassInfo newSystemClass(String className) {
		return new ClassInfo(className, true);
	}
	
	public static ClassInfo newUserClass(String className) {
		return new ClassInfo(className, false);
	}
	
	public String _className;

	public boolean _isSystemClass;

	public ClassInfo _superClass;

	public FieldInfo[] _fields;
	
	public ClassInfo() {
	}
	
	private ClassInfo(String className, boolean systemClass) {
		_className = className;
		_isSystemClass = systemClass;
	}

	public FieldInfo[] getFields() {
		return _fields;
	}

	public void setFields(FieldInfo[] fields) {
		this._fields = fields;
	}

	public ClassInfo getSuperClass() {
		return _superClass;
	}

	public void setSuperClass(ClassInfo superClass) {
		this._superClass = superClass;
	}

	public String getClassName() {
		return _className;
	}

	public boolean isSystemClass() {
		return _isSystemClass;
	}
}
