package com.db4o.omplus.datalayer.queryresult;

import java.util.*;

import com.db4o.*;
import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;


public class ObjectTreeBuilder {
	
	private final String PRMITVE_ARRAY = "[";
	private final String END_ARRAY = "]";
	private final String GENERIC_ARRAY = "(GA)";
	private final String KEY = "key";
	private final String VALUE = "value";
	
	private final IDbInterface db;
	
	public ObjectTreeBuilder(IDbInterface db){
		this.db  = db; 
	}
	
	private ArrayList<Object> modifiedObjList = new ArrayList<Object>();
	private HashMap<Object, Integer> updateDepth = new HashMap<Object, Integer>();

	public ObjectTreeNode getObjectTreeRootNode(String className, Object resultObj){
		ObjectTreeNode newNode = new ObjectTreeNode(db.reflectHelper());
		newNode.setType(className);
		newNode.setValue(resultObj);
		newNode.setName(className);
		newNode.setNodeType(OMPlusConstants.COMPLEX);
		return newNode;
	}
	
	public ObjectTreeNode[] getObjectTree(ObjectTreeNode newNode){
		return getObjectTree(newNode.getType(), newNode.getValue(), newNode);
	}

	public ObjectTreeNode[] getObjectTree(String className, Object resultObj, ObjectTreeNode parent){
		ObjectTreeNode []nodes = null;
		if ( className != null && resultObj != null)
		{
			db.activate(resultObj, 1);
			int index = 0;
			// TODO: check for (GA) in classname
			if(className.startsWith(GENERIC_ARRAY))
				nodes = makeArrayNode(resultObj, parent);
			else
			{
				ReflectClass clazz = getReflectClazz(resultObj);
				if(clazz.isArray())
				{
					nodes = makeArrayNode(resultObj, parent);
				} 
				else if (clazz.isCollection())
				{
					nodes = makeCollectionNode(resultObj, parent);
				} 
				else if( !(clazz.isPrimitive() || ReflectHelper.isWrapperClass(className)))
				{
					// Reflect API can be avoided here.
					ReflectField []fields = ReflectHelper.getDeclaredFieldsInHierarchy(clazz);
					int length = fields.length;
					nodes = new ObjectTreeNode[length];
					for( ReflectField rField : fields )
					{
						ObjectTreeNode newNode = createObjectNode(rField, resultObj, parent);
						//newNode.setParent(parent);
						nodes[index++] = newNode;
					}
				}
				return nodes;
			}
		}
		return nodes;
	}
	
	public void addToModifiedList(ObjectTreeNode node, String[] fieldNames, Object[] values){
		if( node != null ){
			Object prev = node.getOldValue();
			ObjectTreeNode parentNode = node.getParent();
			Object parentObj = parentNode.getValue();
			Object value = node.getValue();
			String type = node.getType();
			String name = getFieldName(node.getName());
			ReflectClass claz = getReflectClazz(parentObj);
			ReflectField field = getReflectField(claz, name);
			if(type.equals(Date.class.getName()))
			{ 
				if(value != null )
				{
					field.set(parentObj, value);
					node.setValue(value);
					if(fieldNames != null && values != null)
					{
						int i = 0;
						for(String fname : fieldNames)
						{
							if(fname.equals(node.getName()))
							{
								values[i] = value;
								break;
							}
							i++;
						}
					}
				}
			} 
			else {
				field.set(parentObj, new Converter().getValue(type, value.toString()));
				node.setValue(value);
			}
			if( parentObj == null || modifiedObjList.contains(parentObj))
			{
				return;
			}
			// added this code for updating primitive arrays.
			int count = 1;
			if( parentNode.getType().startsWith(PRMITVE_ARRAY) ) 
			{ 
				int index = getIndex(node.getName());
				if(index > -1) // move this to DBInterface
					db.getDB().ext().reflector().array().set(parentObj, index, prev);
			}
			while(parentNode != null && parentNode.getNodeType() != 3)
			{
				parentNode = parentNode.getParent();
				parentObj = parentNode.getValue();
				count++;
			}
			if(count > 1){
				updateDepth.put(parentObj, count);
			}
			modifiedObjList.add(parentObj);
		}
	}
	
	public void writeToDB()
	{
		int size = modifiedObjList.size();
		if( size > 0){
			ObjectContainer oc = db.getDB();
			ListIterator<Object> iter = modifiedObjList.listIterator();
			while(iter.hasNext()){
				Object obj = iter.next();
				if(updateDepth.containsKey(obj)){
					oc.ext().store(obj, updateDepth.get(obj));
				}
				else
					oc.store(obj);
			}
			oc.commit();
			// remove all obj in modifiedObjlist
			removeAll();
		}
	}
	
	public boolean isObjectTreeModified(){
		if(modifiedObjList.size() > 0)
			return true;
		else
			return false;
	}
	
	// Check remove(index) why it's implemented this way?
	public void removeAll(){
		int length = modifiedObjList.size();
		for(int count = length - 1; count >= 0; count--){
			modifiedObjList.remove(count);
		}
		updateDepth.clear();
	}

	private String getFieldName(String name) {
		if(name != null && name.trim().length()>0){
			String hierarchy[] = name.split(OMPlusConstants.REGEX);
			int length = hierarchy.length;
			if(length > 1)
				return hierarchy[length - 1];
		}
		return name;
	}

	private ReflectField getReflectField(ReflectClass clz, String fieldName) {
		return clz.getDeclaredField(fieldName);
	}

	private ObjectTreeNode[] makeCollectionNode(Object resultObj, ObjectTreeNode parent) {
		ObjectTreeNode []nodes = null;
//		db.activate(resultObj, 2);
		if (resultObj instanceof Map) {
			return makeMapNode(resultObj, parent);
		} else {
			Collection collection = (Collection)resultObj;
			int size = collection.size();
			int count = 0;
			nodes = new ObjectTreeNode[size];
			if(size > 0){
				Iterator iterator = collection.iterator();
				while(iterator.hasNext()) {
					Object obj = iterator.next();
					ObjectTreeNode newNode = new ObjectTreeNode(db.reflectHelper());
					StringBuilder sb = new StringBuilder();
					sb.append(PRMITVE_ARRAY).append(count).append(END_ARRAY);
					newNode.setName(sb.toString());
					newNode.setValue(obj);
					newNode.setType(getReflectClazz(obj).getName());
					newNode.setParent(parent);
//					Check if it's required to know whether the array items are primitive?
					newNode.setPrimitive(isPrimitive(obj));
					nodes[count++] = newNode;
				}
			}
			return nodes;
		}
	}

	private boolean isPrimitive(Object obj) {
		ReflectClass cls =getReflectClazz(obj);
		return ( cls.isPrimitive() || ReflectHelper.isWrapperClass(cls.getName()));
	}

	private ObjectTreeNode[] makeMapNode(Object resultObj, ObjectTreeNode parent) {
		ObjectTreeNode []nodes = null;
		Map map = (Map)resultObj;
		int size = map.size();
		if( size > 0){
			nodes = new ObjectTreeNode[size * 2];
			int count = 0;
			for (Iterator iterator = map.keySet().iterator(); iterator.hasNext();)
			{
				Object key = iterator.next();
				Object obj = map.get(key);
				ObjectTreeNode keyNode = new ObjectTreeNode(db.reflectHelper());
				keyNode.setName(KEY);
				keyNode.setValue(key);
				keyNode.setPrimitive(isPrimitive(key));
				keyNode.setParent(parent);
				keyNode.setType(getReflectClazz(key).getName());
				nodes[count++] = keyNode;
				ObjectTreeNode newNode = new ObjectTreeNode(db.reflectHelper());
				newNode.setName(VALUE);
				newNode.setValue(obj);
				newNode.setPrimitive(isPrimitive(obj));
				newNode.setParent(parent);
				newNode.setType(getReflectClazz(obj).getName());
				nodes[count++] = newNode;
			}
		}
		return nodes;
	}

	private ObjectTreeNode[] makeArrayNode(Object resultObj, ObjectTreeNode parent) {
		int index = 0;
//		db.activate(resultObj, 2);
		int length = reflector().array().getLength(resultObj);
		ObjectTreeNode []nodes = new ObjectTreeNode[length];
		for(int count = 0; count < length; count++){
			ObjectTreeNode newNode = new ObjectTreeNode(db.reflectHelper());
			Object obj = reflector().array().get(resultObj, count);
			StringBuilder sb = new StringBuilder();
			sb.append(PRMITVE_ARRAY).append(count).append("]");
			newNode.setName(sb.toString());
			
			if(obj != null)
			{
				newNode.setValue(obj);
				newNode.setType(getReflectClazz(obj).getName());
				newNode.setPrimitive(isPrimitive(obj));
			}else
			{
				newNode.setValue(OMPlusConstants.NULL_VALUE);
				newNode.setType("");
				newNode.setPrimitive(false);
			}
				
			newNode.setParent(parent);
//			Check if it's required to know whether the array items are primitive?
			nodes[index++] = newNode;
		}
		return nodes;
	}

	private ObjectTreeNode createObjectNode(ReflectField rField, Object obj, ObjectTreeNode parent) {
		ObjectTreeNode newNode = new ObjectTreeNode(db.reflectHelper());
		ReflectClass type = rField.getFieldType();
		if(parent != null){
			StringBuilder sb = new StringBuilder();
			sb.append(parent.getName());sb.append(OMPlusConstants.REGEX);
			sb.append(rField.getName());
			newNode.setName(sb.toString());
		}else
			newNode.setName(rField.getName());
		newNode.setType(type.getName());
		Object value = rField.get(obj);
//		If value null sending "null" string
		if(value == null){
			newNode.setValue(OMPlusConstants.NULL_VALUE);
			if(type != null){
				if(type.isPrimitive() || ReflectHelper.isWrapperClass(type.getName())){
					newNode.setNodeType(OMPlusConstants.PRIMITIVE);
					newNode.setPrimitive(true);
				}else if(type.isArray() || type.isCollection())
					newNode.setNodeType(OMPlusConstants.COLLECTION);
				else
					newNode.setNodeType(OMPlusConstants.COMPLEX);
			}
		}else {
			newNode.setValue(value);
			boolean isPrimitive = isPrimitive(value);
			newNode.setPrimitive(isPrimitive);
			if(isPrimitive)
				newNode.setNodeType(OMPlusConstants.PRIMITIVE);
			else if(value instanceof Collection || value instanceof Map || value instanceof Map.Entry){
				newNode.setNodeType(OMPlusConstants.COLLECTION);
				db.activate(value, 1);
			} else if( value instanceof GenericObject ) {
				newNode.setNodeType(OMPlusConstants.COMPLEX);
				db.activate(value, 1);
			} else {
				ReflectClass clazz = getReflectClazz(value);
				if(clazz.isArray()) {
					newNode.setNodeType(OMPlusConstants.COLLECTION);
					db.activate(value, 1);
				}
			}
		}
		newNode.setParent(parent);
		return newNode;
	}

	private ReflectClass getReflectClazz(Object obj){
		return reflector().forObject(obj);
	}

	public void setNodeToNull(ObjectTreeNode node)
	{
		if(node == null)
			return;
		ObjectTreeNode parent = node.getParent();
		if(parent != null)
		{
			int nodeType = parent.getNodeType();
			Object parentObj = parent.getValue();
			if(parentObj == null)
				return;
			if( nodeType == OMPlusConstants.COLLECTION )
			{ // ignoring Map and arrays(except String[])
				String type = parent.getType();
				if(type.startsWith(GENERIC_ARRAY) || type.equals("[Ljava.lang.String;"))
				{ // Add support for String arrays
					int index =  getIndex(node.getName());
					reflector().array().set(parentObj, index, null);
				}
//				Do nothing for Collections
				/*else if(parentObj instanceof Collection) {
					int index =  getIndex(node.getName());
					Collection collection = (Collection)parentObj;
					if(index > -1 && index < collection.size())
						collection.remove(node.getValue());
				}*/
			}
			else {
				String fieldName = getFieldName(node.getName());
				if(parentObj != null) {
					ReflectField field = ReflectHelper.getDeclaredFieldInHeirarchy(
							getReflectClazz(parentObj), fieldName);
					field.set(parentObj, null);
				}
			}
			ObjectContainer oc = db.getDB();
			oc.store(parentObj);
			oc.commit();
		}
		else 
		{ // equivalent to delete operation in table & tree;
			if(node.getValue() != null)
			{
				IDbInterface db = Activator.getDefault().dbModel().db();
				db.getDB().delete(node.getValue());
			}
		}
	}

	private int getIndex(String name) {
		int length = name.length();
		return new Integer(name.substring(1, length-1)).intValue();
	}

	public void refresh() {
		int size = modifiedObjList.size();
		if( size > 0){
			ObjectContainer oc = db.getDB();
			ListIterator<Object> iter = modifiedObjList.listIterator();
			while(iter.hasNext()){
				Object obj = iter.next();
				oc.ext().refresh(obj, 2);
			}
			// remove all obj in modifiedObjlist
			removeAll();
		}
		
	}

	private Reflector reflector() {
		return db.getDB().ext().reflector();
	}
}
