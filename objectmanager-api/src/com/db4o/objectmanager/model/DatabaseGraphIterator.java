/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model;


import java.util.LinkedList;

import com.db4o.objectmanager.model.nodes.ClassNode;
import com.db4o.objectmanager.model.nodes.IModelNode;
import com.db4o.reflect.ReflectClass;

/**
 * DatabaseGraphIterator.  A visitor that can traverse the contents of an 
 * object database file.
 *
 * @author djo
 */
public class DatabaseGraphIterator extends AbstractGraphIterator {
    
	/**
     * (non-API)
     * Constructor DatabaseGraphIterator.  Constructs a DatabaseGraphIterator that can
     * traverse all the objects in a database graph.
     * 
     * @param database The Database to traverse
     * @param classes The StoredClasses to consider as the root
     */
    public DatabaseGraphIterator(IDatabase database, ReflectClass[] start) {
        this.database = database;
        
        LinkedList results = new LinkedList();
        for (int i = 0; i < start.length; i++) {
            if (database.instanceIds(start[i]).length > 0)
                results.add(new ClassNode(start[i], database));
        }
        startModel = (IModelNode[]) results.toArray(new IModelNode[results.size()]);
        reset();
    }
}
