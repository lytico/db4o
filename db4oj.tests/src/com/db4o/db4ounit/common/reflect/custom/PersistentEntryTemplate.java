package com.db4o.db4ounit.common.reflect.custom;

public class PersistentEntryTemplate {

	public String className;

	public String[] fieldNames;

	public Object[] fieldValues;

	public PersistentEntryTemplate(String className, String[] fieldNames, Object[] fieldValues)
	{
		this.className = className;
		this.fieldNames = fieldNames;
		this.fieldValues = fieldValues;
	}
}
