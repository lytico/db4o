package com.db4o.objectmanager.model;

import com.db4o.objectmanager.model.nodes.field.InstanceNode;
import com.db4o.objectmanager.model.nodes.IModelNode;

/**
 * Used to traverse through a single object.
 *
 * User: treeder
 * Date: Sep 8, 2006
 * Time: 12:57:16 AM
 */
public class ObjectGraphIterator extends AbstractGraphIterator {
    public ObjectGraphIterator(Object o, IDatabase db) {
        this.database = db;
        startModel = new IModelNode[1];
        InstanceNode node = new InstanceNode(o, db);
        startModel[0] = node;
        reset();
    }
}
