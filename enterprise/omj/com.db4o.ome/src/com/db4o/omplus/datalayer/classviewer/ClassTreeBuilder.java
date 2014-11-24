package com.db4o.omplus.datalayer.classviewer;

import java.util.*;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.reflect.*;

public class ClassTreeBuilder {
	
	private final static String DEFAULT_PACKAGE = "default";
	
	public ClassTreeBuilder() {
	}
	
	public ClassTreeNode [] getClassNodesForPackage(String packageName){
		if(packageName == null)
			return null;
		ClassTreeNode [] nodes = null;
		ArrayList<String> list = getClassesForPackage(packageName);
		if(list != null) {
			int i = 0;
			nodes = new ClassTreeNode[list.size()];
			ListIterator<String> iterator = list.listIterator();
			while(iterator.hasNext()){
				String name = iterator.next();
				nodes[i] = buildClassNode(name);
				i++;
			}
		}
		return nodes;
	}
	
	public ClassTreeNode [] getFieldNodesForClass(String className, String hierarchy) {
		ClassTreeNode [] nodes = null;
		if(className != null ){
			ReflectClass clazz = db().reflectHelper().getReflectClazz(className);
			if(clazz != null) {
				ReflectField[] fields = ReflectHelper.getDeclaredFieldsInHierarchy(clazz);
				if(fields != null) {
					nodes = new ClassTreeNode[fields.length];
					int i = 0;
					for(ReflectField rField : fields){
						nodes[i] = buildFieldNode(rField, hierarchy);
						i++;
					}
				}
			}
		}
		return nodes;
	}
	
	private ClassTreeNode buildFieldNode(ReflectField field, String hierarchy) {
		if(field == null && hierarchy == null)
			return null;
		ClassTreeNode node = new ClassTreeNode(db().reflectHelper());
		node.setName(append(hierarchy, field.getName()));
		node.setNodeType(OMPlusConstants.CLASS_FIELD_NODE);
		ReflectClass type = field.getFieldType();
		String typeName = type.getName();
		node.setType(typeName);
		boolean hasChildren = !(type.isPrimitive() || ReflectHelper.isWrapperClass(typeName)
				|| type.isArray() || type.isCollection() 
				|| typeName.equals(StringBuffer.class.getName()) 
				|| typeName.equals(StringBuilder.class.getName()) );
		node.setHasChildren(hasChildren);
		return node;
	}

	private String append(String hierarchy, String name) {
		StringBuilder sb = new StringBuilder(hierarchy);
		sb.append(OMPlusConstants.REGEX);
		sb.append(name);
		return sb.toString();
	}

	public ClassTreeNode [] getClassTreeNodes(){
		Object[] classes = getStoredClasses();
		ClassTreeNode [] nodes = null;
		if(classes != null){
			int length = classes.length;
			int count = 0;
			nodes = new ClassTreeNode[length];
			for(Object className : classes){
				nodes[count++] = buildClassNode((String)className);
			}
		}
		return nodes;
	}
	
	// ignored parent for class. check if it's needed
	private ClassTreeNode buildClassNode(String className){
		ClassTreeNode node = new ClassTreeNode(db().reflectHelper());
		node.setName((String)className);
		node.setType((String)className);
		node.setHasChildren(true);
		node.setNodeType(OMPlusConstants.CLASS_NODE);
		return node;
	}
	
	public ClassTreeNode [] getPackageTreeNodes(){
		Object[] classes = getStoredClasses();
		ClassTreeNode [] nodes = null;
		if(classes != null){
			Object[] packageNames = getPackageNames(classes);
			int length = packageNames.length;
			int count = 0;
			nodes = new ClassTreeNode[length];
			for(Object pName : packageNames){
				nodes[count++] = buildPackageNode((String)pName);
			}
		}
		return nodes;
	}
	
	private ClassTreeNode buildPackageNode(String packageName) {
		ClassTreeNode node = new ClassTreeNode(db().reflectHelper());
		node.setName((String)packageName);
		node.setHasChildren(true);
		node.setNodeType(OMPlusConstants.PACKAGE_NODE);
		return node;
	}

	private Object[] getPackageNames(Object []classes) {
		String packageName = null;
		HashSet<Object> set = new HashSet<Object>(classes.length);
		for(Object name : classes) {
			packageName = getPackageName((String)name);
			if(packageName != null && packageName.trim().length() > 0)
				set.add(packageName);
		}
		return set.toArray();
	}
	
	public String getPackageName(String name){
		int index = -1;
		String packageName = null;
		index = ((String)name).lastIndexOf(OMPlusConstants.DOT_OPERATOR);
		if(index > 0) {
			packageName = ((String)name).substring(0, index);
		}
		else // added for classes without package
			packageName = DEFAULT_PACKAGE;
		return packageName;
	}
	
	private ArrayList<String> getClassesForPackage(String packageName) {
		if(packageName != null){
			ArrayList<String> list = new ArrayList<String>();
			Object[] classes = getStoredClasses();
			if(classes != null){
				for( Object name : classes){
					String pName = getPackageName((String)name);
					if( pName.equals(packageName)){
						list.add((String)name);
					}
				}
				return list;
			}
		}
		return null;
	}

	public Object [] getStoredClasses() {
		return db().getStoredClasses();
	}
	
	private IDbInterface db() {
		return Activator.getDefault().dbModel().db();
	}
}
