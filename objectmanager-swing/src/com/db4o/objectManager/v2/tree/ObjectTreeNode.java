package com.db4o.objectManager.v2.tree;

import com.db4o.reflect.ReflectField;
import com.db4o.objectManager.v2.MainPanel;

import java.util.Collection;
import java.util.Map;
import java.util.Date;
import java.lang.reflect.Array;

/**
 * User: treeder
 * Date: Sep 8, 2006
 * Time: 11:32:24 AM
 */
public class ObjectTreeNode {
	private ObjectTreeNode parentNode;
	private ReflectField field;
	private int index = -1;
	private Object ob;

	public ObjectTreeNode(ObjectTreeNode parentNode, ReflectField field, Object ob) {
		this.parentNode = parentNode;
		this.field = field;
		this.ob = ob;
	}

	/**
	 * Use this constructor for collections and arrays.
	 * @param parentNode
	 * @param index
	 * @param ob
	 */
	public ObjectTreeNode(ObjectTreeNode parentNode, int index, Object ob) {
		this.parentNode = parentNode;
		this.index = index;
		this.ob = ob;
	}

	public Object getObject() {
		return ob;
	}

	public String toString() {
		String ret = "";
		try {
			if (field != null) {
				ret = field.getName() + ": ";
			} else ret = "";
			if (ob == null) ret += ob;
			else if (ob instanceof Date){
				ret += MainPanel.dateFormatter.display((Date) ob);
			} else if (ob.getClass().isArray()) {
				ret += "Array[" + Array.getLength(ob) + "]";
			} else if (ob instanceof Collection) {
				Collection collection = (Collection) ob;
				ret += "Collection[" + collection.size() + "]";
			} else if (ob instanceof Map) {
				Map map = (Map) ob;
				ret += "Map[" + map.size() + "]";
			} else {
				ret += ob;
			}
		} catch (Exception e) {
			ret = ob.toString();
		}
		return ret;

	}

	public void setObject(Object object) {
		this.ob = object;
	}

	public ObjectTreeNode getParentNode() {
		return parentNode;
	}

	public ReflectField getField() {
		return field;
	}

	public int getIndex() {
		return index;
	}
}
