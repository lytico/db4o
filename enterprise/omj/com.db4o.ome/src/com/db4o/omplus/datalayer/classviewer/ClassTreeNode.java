package com.db4o.omplus.datalayer.classviewer;

import com.db4o.omplus.datalayer.OMPlusConstants;
import com.db4o.omplus.datalayer.ReflectHelper;
import com.db4o.reflect.ReflectClass;

public class ClassTreeNode {
	
	private String name;
	
	private String type;
	
	//TODO: to be removed. No need for this
//	private ClassTreeNode parent;
	
	private int nodeType;
	
	private boolean hasChildren;

	private final ReflectHelper reflectHelper;
	
	public ClassTreeNode(ReflectHelper reflectHelper) {
		this.reflectHelper = reflectHelper;
	}
	
	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getType() {
		return type;
	}

	public void setType(String type) {
		this.type = type;
	}

	public int getNodeType() {
		return nodeType;
	}

	public void setNodeType(int nodeType) {
		this.nodeType = nodeType;
	}

	public boolean hasChildren() {
		return hasChildren;
	}

	public void setHasChildren(boolean hasChildren) {
		this.hasChildren = hasChildren;
	}

	public int getFieldNodeType() {
		if(type != null){
			if(type.contains("(GA")){
				return OMPlusConstants.COLLECTION;
			}
			ReflectClass clazz = reflectHelper.getReflectClazz(type);
			if(clazz != null){
				if(clazz.isPrimitive() || ReflectHelper.isWrapperClass(type))
					return OMPlusConstants.PRIMITIVE;
				else if (clazz.isArray() || clazz.isCollection())
					return OMPlusConstants.COLLECTION;
				else
					return OMPlusConstants.COMPLEX;
			}
		}
		return OMPlusConstants.PRIMITIVE;
	}

/*	public ClassTreeNode getParent() {
		return parent;
	}

	public void setParent(ClassTreeNode parent) {
		this.parent = parent;
	}*/

}
