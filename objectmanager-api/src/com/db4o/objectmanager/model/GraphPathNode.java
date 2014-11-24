/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model;

import com.db4o.objectmanager.model.nodes.IModelNode;

/**
 * GraphPathNode.  Maintains one step in the path to the current tree node.
 *
 * @author djo
 */
public class GraphPathNode {
	/**
	 * Non-API
	 * 
	 * @param children
	 * @param selectedChild
	 */
	public GraphPathNode(IModelNode[] children, int selectedChild) {
		this.children = children;
		this.selectedChild = selectedChild;
	}

	/**
	 * Non-API
	 */
    public final IModelNode[] children;
	
	/**
	 * Non-API
	 */
    public final int selectedChild;

}
