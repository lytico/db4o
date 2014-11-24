/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model;


import com.db4o.ObjectSet;
import com.db4o.objectmanager.model.nodes.partition.PartitionFieldNodeFactory;

/**
 * ObjectSetGraphIterator.  A visitor that can traverse the contents of an 
 * object database file.
 *
 * @author djo
 */
public class ObjectSetGraphIterator extends AbstractGraphIterator {
    
	/**
     * (non-API)
     * Constructor ObjectSetGraphIterator.  Constructs a ObjectSetGraphIterator that can
     * traverse all the objects in a database graph.
     * 
     * @param database The Database to traverse
     * @param classes The StoredClasses to consider as the root
     */
    public ObjectSetGraphIterator(IDatabase database, ObjectSet queryResult) {
        this.database = database;
        startModel = PartitionFieldNodeFactory.create(queryResult, null, 0, queryResult.size(), database);
        reset();
    }
}
