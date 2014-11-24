/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model.nodes.field;

import java.io.PrintStream;

import com.db4o.objectmanager.model.IDatabase;
import com.db4o.objectmanager.model.nodes.IModelNode;
import com.db4o.objectmanager.model.nodes.partition.PartitionFieldNodeFactory;
import com.db4o.reflect.ReflectArray;
import com.db4o.reflect.ReflectClass;

/**
 * ArrayFieldNode.
 *
 * @author djo
 */
public class ArrayFieldNode extends FieldNode {
    
    private int length;
    private ReflectArray arrayReflector;

	public ArrayFieldNode(String fieldName, ReflectClass fieldType,Object instance, IDatabase database) {
		super(fieldName, fieldType,instance, database);

        arrayReflector = _database.reflector().array();
        length = arrayReflector.getLength(value);
	}

	/* (non-Javadoc)
	 * @see com.db4o.objectmanager.model.nodes.field.StoredFieldNode#children()
	 */
	public IModelNode[] children() {
        
        IModelNode[] result = new IModelNode[length];
        
        for (int i=0; i < length; ++i) {
            Object item = arrayReflector.get(value, i);
            result[i] = FieldNodeFactory.construct("["+ i + "] ", _fieldType.getComponentType(), item, _database);
        }
        
        return PartitionFieldNodeFactory.create(result,0,result.length,_database);
	}

	/* (non-Javadoc)
	 * @see com.db4o.objectmanager.model.nodes.field.StoredFieldNode#hasChildren()
	 */
	public boolean hasChildren() {
		return length > 0;
	}
	
	public String getText() {
        return _fieldName + " " + _database.reflector().forObject(value).getName();
	}
    
    public boolean isEditable() {
        return false;
    }

	public void printXmlValueNode(PrintStream out) {
		out.print("<" + getNodeName() + " id=\"" + getId() + "\">");
		out.print("</" + getNodeName() + ">");
	}

	public boolean shouldIndent() {
		return true;
	}

}
