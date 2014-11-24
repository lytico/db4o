package com.db4o.omplus.datalayer.propertyViewer.classProperties;

public class ClassProperties {

	private String classname;
	
	private	int numberOfObjects;
	
	private FieldProperties []fields;

	public String getClassname() {
		return classname;
	}

	public void setClassname(String classname) {
		this.classname = classname;
	}

	public int getNumberOfObjects() {
		return numberOfObjects;
	}

	public void setNumberOfObjects(int numberOfObjects) {
		this.numberOfObjects = numberOfObjects;
	}

	public FieldProperties[] getFields() {
		return fields;
	}

	public void setFields(FieldProperties[] fields) {
		this.fields = fields;
	}
	
//	TODO: Functions to be added to avoid Reflection code.
	
}
