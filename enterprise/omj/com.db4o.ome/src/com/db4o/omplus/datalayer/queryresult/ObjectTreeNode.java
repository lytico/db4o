package com.db4o.omplus.datalayer.queryresult;

import com.db4o.omplus.datalayer.OMPlusConstants;
import com.db4o.omplus.datalayer.ReflectHelper;

public class ObjectTreeNode {
	
	private	String	name;
	private	String	type;
	private Object	value;
	private Object	oldValue;
	private	boolean	isPrimitive;
	private ObjectTreeNode parent;
	
	private int nodeType;
	private final ReflectHelper reflectHelper;

	public ObjectTreeNode(ReflectHelper reflectHelper) {
		this.reflectHelper = reflectHelper;
		
	}
	
	public String getName() {
		return name;
	}
	public void setName(String fieldName) {
		this.name = fieldName;
	}
	public String getType() {
		return type;
	}
	public void setType(String fieldType) {
		this.type = fieldType;
	}
	public Object getValue() {
		return value;
	}
	public void setValue(Object value) {
		this.value = value;
	}
	public boolean isPrimitive() {
		return isPrimitive;
	}
	public void setPrimitive(boolean isPrimitive) {
		this.isPrimitive = isPrimitive;
	}
	public ObjectTreeNode getParent() {
		return parent;
	}
	public void setParent(ObjectTreeNode parent) {
		this.parent = parent;
	}
//	TODO: to be removed
	public String toString()
	{
		return (name+"  .... "+value);
	}
	public Object getOldValue() {
		return oldValue;
	}
	public void setOldValue(Object oldValue) {
		this.oldValue = oldValue;
	}
	
	/**
	 * RTEURNS IF THIS NODE IS PRIMITVE/COLLECTION/COMPLEX TYPE
	 * @return
	 */
	public int getNodeType() 
	{
		//TODO: add correct logic here
		return nodeType;
	}
	
	public void setNodeType( int i) 
	{
		this.nodeType = i;
	}
	
	public int getArrayLength() {
		if( value == null || value.equals(OMPlusConstants.NULL_VALUE))
			return 0;
		else {
			return reflectHelper.getArraySize(value);
			
		}
			
	}
}
