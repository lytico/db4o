/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal;

public class FieldInfo {

	public String _fieldName;

	public ClassInfo _fieldClass;

	public boolean _isPrimitive;

	public boolean _isArray;

	public boolean _isNArray;

	public FieldInfo() {
	}

	public FieldInfo(String fieldName, ClassInfo fieldClass,
			boolean isPrimitive, boolean isArray, boolean isNArray) {
		_fieldName = fieldName;
		_fieldClass = fieldClass;
		_isPrimitive = isPrimitive;
		_isArray = isArray;
		_isNArray = isNArray;
	}

	public ClassInfo getFieldClass() {
		return _fieldClass;
	}

	public String getFieldName() {
		return _fieldName;
	}
}
