package com.db4o.objectManager.v2.tree;

import java.lang.reflect.*;
import java.util.*;

import javax.swing.event.*;
import javax.swing.tree.*;

import com.db4o.objectManager.v2.*;
import com.db4o.objectManager.v2.util.*;
import com.db4o.objectmanager.api.helpers.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;
import com.db4o.ta.*;
import com.spaceprogram.db4o.sql.util.*;

/**
 * User: treeder
 * Date: Sep 8, 2006
 * Time: 10:52:27 AM
 */
public class ObjectTreeModel implements TreeModel {
	private ObjectTreeNode root;
	private UISession session;
	protected EventListenerList listenerList = new EventListenerList();


	public ObjectTreeModel(ObjectTreeNode top, UISession session) {
		this.root = top;
		this.session = session;
	}

	public Object getRoot() {
		return root;
	}

	public Object getChild(Object parent, int index) {
		ObjectTreeNode parentNode = (ObjectTreeNode) parent;
		Object parentObject = parentNode.getObject();
		Reflector reflector = session.getObjectContainer().ext().reflector();
		ReflectClass reflectClass = reflector.forObject(parentObject);
		if(Activatable.class.isAssignableFrom(parentObject.getClass())){
			((Activatable)parentObject).activate();
		}		
		
		if (reflectClass.isArray()) {
			return makeArrayNode(parentObject, parentNode, index, reflector);
		} else if (reflector.isCollection(reflectClass)) {
			// reflector.isCollection returns true for Maps too
			if (parentObject instanceof Map) {
				return makeMapNode(parentObject, parentNode, index);
			} else {
				return makeCollectionNode(parentObject, parentNode, index);
			}
		} else if (parentObject instanceof MapEntry) {
			MapEntry entry = (MapEntry) parentObject;
			if (index == 0) {
				return new ObjectTreeNode(parentNode, index, entry.getEntry().getKey());
			} else {
				return new ObjectTreeNode(parentNode, index, entry.getEntry().getValue());
			}
		} else {
			// todo: could try caching all this reflect information if performance is bad -> get from ObjectSetMetaData
			ReflectField[] fields = ReflectHelper.getDeclaredFieldsInHeirarchy(reflectClass);
			fields[index].setAccessible();
			Object value = fields[index].get(parentObject);
			//System.out.println("getChild parent:" + parentNode.getObject().getClass() + " index:" + index + " field:" + fields[index].getName() + " value:" + value);
			return new ObjectTreeNode(parentNode, fields[index], value);
		}
	}

	private Object makeArrayNode(Object parentObject, ObjectTreeNode parentNode, int index, Reflector reflector) {
		return new ObjectTreeNode(parentNode, index, reflector.array().get(parentObject, index));
	}

	private Object makeCollectionNode(Object parentObject, ObjectTreeNode parentNode, int index) {
		Collection collection = (Collection) parentObject;
		int i = 0;
		Object toUse = null;
		for (Iterator iterator = collection.iterator(); iterator.hasNext();) {
			Object o = iterator.next();
			if (i == index) {
				toUse = o;
				break;
			}
			i++;
		}
		return new ObjectTreeNode(parentNode, index, toUse);
	}

	private Object makeMapNode(Object parentObject, ObjectTreeNode parentNode, int index) {
		Map map = (Map) parentObject;
		Object[] arr = map.entrySet().toArray(); // todo: this may be poor performance, should do something else
		return new ObjectTreeNode(parentNode, index, new MapEntry((Map.Entry) arr[index]));
	}

	public int getChildCount(Object parent) {
		ObjectTreeNode parentNode = (ObjectTreeNode) parent;
		Reflector reflector = session.getObjectContainer().ext().reflector();
		ReflectClass reflectClass = reflector.forObject(parentNode.getObject());

		if (reflectClass.isArray()) {
			//System.out.println("is array " + parentNode.getObject().getClass());
			return Array.getLength(parentNode.getObject());
		} else if (parentNode.getObject() instanceof Collection) {
			Collection collection = (Collection) parentNode.getObject();
			return collection.size();
		} else if (parentNode.getObject() instanceof Map) {
			Map map = (Map) parentNode.getObject();
			return map.size();
		} else if (parentNode.getObject() instanceof MapEntry) {
			return 2;
		}

		ReflectField[] fields = ReflectHelper.getDeclaredFieldsInHeirarchy(reflectClass);
		return fields.length;
	}

	public boolean isLeaf(Object node) {
		Reflector reflector = session.getObjectContainer().ext().reflector();
		if (node == null || ((ObjectTreeNode) node).getObject() == null) return true;
		Object nodeObject = ((ObjectTreeNode) node).getObject();
		if (nodeObject instanceof GenericObject) {
			GenericObject go = (GenericObject) nodeObject;
			ReflectClass gclass = reflector.forObject(nodeObject);
			//System.out.println("GENOB: " + go + " class:" + gclass.getName());
			if (gclass.getName().contains("System.DateTime")) {
				// todo: move this into isEditable
				return true;
			}
		}
		return ReflectHelper2.isEditable(nodeObject.getClass());
	}

	public void valueForPathChanged(TreePath path, Object newValue) {
		ObjectTreeNode aNode = (ObjectTreeNode) path.getLastPathComponent();
//		System.out.println("new value: " + newValue + " " + newValue.getClass() + " old ob: " + aNode.getObject() + " " + aNode.getObject().getClass());
		try {
			Object oldOb = aNode.getObject();
			Class c = aNode.getObject().getClass();
//			System.out.println("class: " + c);
//			System.out.println("isBoolean: " + (Boolean.class.isAssignableFrom(c)));
			Object newOb = convertToObject(c, (String) newValue);
			ObjectTreeNode parentNode = aNode.getParentNode();
			int index = aNode.getIndex();
			Object parentObject = parentNode.getObject();
			ObjectTreeNode superParentNode = parentNode.getParentNode(); // if a collection, then we'll want to store the parent of the collection.
			Reflector reflector = session.getObjectContainer().ext().reflector();
			ReflectClass reflectClass = reflector.forObject(parentObject);
			boolean storeSuper = false;
			if (reflectClass.isArray()) {
				Array.set(parentObject, index, newOb);
				storeSuper = true;
			} else if (reflector.isCollection(reflectClass)) {
				// reflector.isCollection returns true for Maps too I guess
				if (parentObject instanceof Map) {
					Map map = (Map) parentObject;
					MapEntry mapEntry = (MapEntry) aNode.getObject();
					map.put(mapEntry.getEntry().getKey(), newOb);
				} else if (parentObject instanceof List) {
					List collection = (List) parentObject;
					collection.set(index, newOb);
				} else if (parentObject instanceof Set) {
					// set has no guarantee of ordering, so need to remove old, then add
					Set collection = (Set) parentObject;
					collection.add(newOb);
				} else if (parentObject instanceof Queue) {
					// this may not be expected behaviour since it will place the changed object at the end of the Queue, not at the index
					Queue q = (Queue) parentObject;
					System.out.println("removed from queue? " + q.remove(oldOb));
					q.add(newOb);
				}
				storeSuper = true;
			} else if (parentObject instanceof MapEntry) {
				MapEntry entry = (MapEntry) parentObject;
				if (index == 0) {
					Map superMap = (Map) superParentNode.getObject();
					superMap.remove(entry.getEntry().getKey());
					superMap.put(newOb, entry.getEntry().getValue());
				} else {
					Map superMap = (Map) superParentNode.getObject();
					superMap.remove(entry.getEntry().getKey()); // removing just in case
					superMap.put(entry.getEntry().getKey(), newOb);
				}
				superParentNode = superParentNode.getParentNode(); // since we've injected an extra layer with the MapEntry
				storeSuper = true;
			} else {
				ReflectField rf = aNode.getField();
				rf.setAccessible();
				rf.set(parentObject, newOb);
			}
			if (storeSuper) {
				addToBatch(superParentNode.getObject());
			} else {
				addToBatch(parentObject);
			}
			aNode.setObject(newOb);
		} catch (Exception e) {
			e.printStackTrace();
			Log.addException(e);
		}

	}

	private Object convertToObject(Class c, String newValue) throws Exception {
		return Converter.convertFromString(c, newValue);
	}

	private void addToBatch(Object o) {
		// similar to Object
		session.getObjectContainer().set(o);
		session.getObjectContainer().commit();
	}

	public int getIndexOfChild(Object parent, Object child) {
		return 0;
	}

	public void addTreeModelListener(TreeModelListener l) {
		listenerList.add(TreeModelListener.class, l);
	}

	public void removeTreeModelListener(TreeModelListener l) {
		listenerList.remove(TreeModelListener.class, l);
	}

	/**
	 * Only primitive fields (and quasi-primitives) will be editable.
	 *
	 * @param path
	 * @return
	 */
	public boolean isPathEditable(TreePath path) {
		ObjectTreeNode aNode = (ObjectTreeNode) path.getLastPathComponent();
//		System.out.println("aNode:" + aNode + " - object:" + aNode.getObject());
		// todo: should check the expect class type if this is null so you can edit null values
		if (aNode.getObject() == null) return false;
		Class c = aNode.getObject().getClass();
//		System.out.println("class editable: " + c);
		return ReflectHelper2.isEditable(c);
	}
}
