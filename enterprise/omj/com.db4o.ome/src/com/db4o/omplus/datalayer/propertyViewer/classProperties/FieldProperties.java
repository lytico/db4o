package com.db4o.omplus.datalayer.propertyViewer.classProperties;

public class FieldProperties {
	
	private	String	fieldName;
	
	private	String	 fieldDataType;
	
	private	boolean	isPrimitive;
	
	private boolean	isIndexed;
	
	private String	accessModifier;

	public String getAccessModifier() 
	{
		return accessModifier;
	}

	public void setAccessModifier(String str) 
	{
		this.accessModifier = str;
	}

	public String getFieldName() {
		return fieldName;
	}

	public void setFieldName(String name) {
		this.fieldName = name;
	}

	public String getFieldDataType() {
		return fieldDataType;
	}

	public void setFieldDataType(String type) {
		this.fieldDataType = type;
	}

	public boolean isPrimitive() {
		return isPrimitive;
	}

	public void setPrimitive(boolean isPrimitive) {
		this.isPrimitive = isPrimitive;
	}

	public boolean isIndexed() {
		return isIndexed;
	}

	public void setIndexed(boolean isIndexed) {
		this.isIndexed = isIndexed;
	}

}
