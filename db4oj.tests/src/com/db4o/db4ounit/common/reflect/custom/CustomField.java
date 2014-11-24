package com.db4o.db4ounit.common.reflect.custom;

import com.db4o.reflect.*;

/**
 * One important thing to remember when implementing ReflectField
 * is that getFieldType and getIndexType must always return ReflectClass
 * instances given by the parent reflector.
 */
public class CustomField implements ReflectField {

	// fields must be public so test works on less capable runtimes
	public CustomClassRepository _repository;	
	public String _name;
	public Class _type;
	public int _index;
	public boolean _indexed;

	public CustomField() {
	}

	public CustomField(CustomClassRepository repository, int index, String name, Class type) {
		_repository = repository;
		_index = index;
		_name = name;
		_type = type;
	}

	public Object get(Object onObject) {
		logMethodCall("get", onObject);
		return fieldValues(onObject)[_index];
	}

	private Object[] fieldValues(Object onObject) {
		return ((PersistentEntry)onObject).fieldValues;
	}

	public ReflectClass getFieldType() {
		logMethodCall("getFieldType");
		return _repository.forFieldType(_type);
	}

	public String getName() {
		return _name;
	}

	public Object indexEntry(Object orig) {
		logMethodCall("indexEntry", orig);
		return orig;
	}

	public ReflectClass indexType() {
		logMethodCall("indexType");
		return getFieldType();
	}

	public boolean isPublic() {
		return true;
	}

	public boolean isStatic() {
		return false;
	}

	public boolean isTransient() {
		return false;
	}

	public void set(Object onObject, Object value) {
		logMethodCall("set", onObject, value);
		fieldValues(onObject)[_index] = value;
	}

	public void indexed(boolean value) {
		_indexed = value;
	}
	
	public boolean indexed() {
		return _indexed;
	}

	public String toString() {
		return "CustomField(" + _index + ", " + _name + ", " + _type.getName() + ")";
	}

	private void logMethodCall(String methodName) {
		Logger.logMethodCall(this, methodName);
	}

	private void logMethodCall(String methodName, Object arg) {
		Logger.logMethodCall(this, methodName, arg);
	}

	private void logMethodCall(String methodName, Object arg1, Object arg2) {
		Logger.logMethodCall(this, methodName, arg1, arg2);
	}
}
