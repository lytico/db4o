package com.db4o.db4ounit.common.reflect.custom;

import com.db4o.foundation.*;
import com.db4o.reflect.*;

public class CustomClassRepository {

	// fields must be public so test works on less capable runtimes
	public Hashtable4 _classes;
	public transient CustomReflector _reflector;

	public CustomClassRepository() {
		_classes = new Hashtable4();
	}

	public CustomClass forName(String className) {
		return (CustomClass)_classes.get(className);
	}

	public CustomClass defineClass(String className, String[] fieldNames,
			String[] fieldTypes) {

		assertNotDefined(className);
		CustomClass klass = createClass(className, fieldNames, fieldTypes);
		return defineClass(klass);
	}

	private CustomClass createClass(String className, String[] fieldNames,
			String[] fieldTypes) {
		return new CustomClass(this, className, fieldNames, resolveTypes(fieldTypes));
	}

	private Class[] resolveTypes(String[] typeNames) {
		Class[] types = new Class[typeNames.length];
		for (int i=0; i<types.length; ++i) {
			types[i] = resolveType(typeNames[i]);
		}
		return types;
	}

	private Class resolveType(String typeName) {
		if (typeName.equals("string")) {
			return java.lang.String.class;
		}
		if (typeName.equals("int")) {
			return java.lang.Integer.class;
		}
		throw new IllegalArgumentException("Invalid type '" + typeName + "'");
	}

	private CustomClass defineClass(CustomClass klass) {
		_classes.put(klass.getName(), klass);
		return klass;
	}

	private void assertNotDefined(String className) {
		if (_classes.containsKey(className)) {
			throw new IllegalArgumentException("Class '" + className + "' already defined.");
		}
	}

	public void initialize(CustomReflector reflector) {
		_reflector = reflector;
	}

	public ReflectClass forFieldType(Class type) {
		return _reflector.forFieldType(type);
	}

	public String toString() {
		return "CustomClassRepository(classes: " + _classes.size() + ")";
	}

	public Iterator4 iterator() {
		return _classes.valuesIterator();
	}
}