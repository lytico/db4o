package com.db4o.db4ounit.common.reflect.custom;

import com.db4o.foundation.*;


public class PersistentEntry {

	public String className;

	public Object uid;

	public Object[] fieldValues;

	public PersistentEntry() {
	}

	public PersistentEntry(String className, Object uid, Object[] fieldValues)
	{
		this.className = className;
		this.uid = uid;
		this.fieldValues = fieldValues;
	}

	public String toString() {
		return "PersistentEntry(" + className + ", " + uid + ", " + new Collection4(fieldValues) + ")";
	}
}
