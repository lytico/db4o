/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model;

import java.util.Iterator;
import java.util.LinkedList;
import java.util.ListIterator;

import com.db4o.objectmanager.model.nodes.IModelNode;

/**
 * GraphPosition.  Represents the position of some object within the object
 * graph and provides a way to retrieve a handle to such an object.
 *
 * @author djo
 */
public class GraphPosition {
    
    private LinkedList path = new LinkedList();
    
    /*(non-API)
     * Default constructor.
     */
    public GraphPosition() {
        // Nothing needed here...
    }
    
	/**
     * Copy Constructor GraphPosition.  Copy a GraphPosition object and create a
     * new one with the same contents.
     * 
	 * @param path2 The path to copy.
	 */
	public GraphPosition(GraphPosition path2) {
		for (Iterator i = path2.path.iterator(); i.hasNext();) {
			GraphPathNode node = (GraphPathNode) i.next();
			path.add(new GraphPathNode(node.children, node.selectedChild));
		}
	}

	/**
     * Method iterator.  Return an Iterator where each element is an IModelNode
     * representing the object traversed to get to the current element.
     * 
	 * @return Iterator an interator on each path element.
	 */
	public ListIterator iterator() {
        return path.listIterator();
    }
	
	/**
	 * (non-API) Method size.  Used for unit testing only.
	 *  
	 * @return Return the number of stored GraphPathNodes.
	 */
	public int size() {
		return path.size();
	}
    
    /**
     * Method hasParent. Indicates if the current path has parent nodes stored.
     * 
     * @return true if there are parent nodes stored in the GraphPosition, false
     * otherwise.
     */
    public boolean hasParent() {
        return !path.isEmpty();
    }
    
    /**
     * Method push.  Add an IModelNode to the top of the stack.
     * 
     * Put an IModelNode on the GraphPosition stack.
     * 
     * @param node The IModelNode to push
     */
    public void push(IModelNode[] children, int node) {
        path.addLast(new GraphPathNode(children, node));
    }
    
    /**
     * Method pop.  Pop the IModelNode stack.
     * 
     * Removes the most recent item from the IModelNode stack and returns
     * it.
     * 
     * @return The IModelNode to pull
     */
    public GraphPathNode pop() {
        if (path.isEmpty()) {
            throw new IllegalArgumentException("Nothing left to pop from the stack");
        }
        return (GraphPathNode) path.removeLast();
    }
    
    /**
     * Method getCurrent.  Return the current IModelNode in this GraphPosition.
     * 
     * @return IModelNode the current node or null if the database is empty.
     */
    public IModelNode getCurrent() {
        GraphPathNode current = (GraphPathNode) path.getLast();
		if (current.children.length > 0) {
			return current.children[current.selectedChild+1];
		} else {
			return null;
		}
    }
    
    /* (non-Javadoc)
	 * @see java.lang.Object#equals(java.lang.Object)
	 */
	public boolean equals(Object obj) {
        if (!(obj instanceof GraphPosition)) {
            return false;
        }
        
        // We consider the other object to be equal if it followed the same
        // path to get to its current object.
        GraphPosition other = (GraphPosition) obj;
        
        if (other.path.size() != path.size()) {
            return false;
        }

		// The path to the current object is defined by the sequence of
		// parent indices stored in the path.selectedChild lists
        Iterator iOther = other.path.iterator();
        Iterator iSelf = path.iterator();
        
        while (iSelf.hasNext()) {
            GraphPathNode nOther = (GraphPathNode) iOther.next();
            GraphPathNode nSelf = (GraphPathNode) iSelf.next();
            
            if (nOther.selectedChild != nSelf.selectedChild) {
                return false;
            }
        }
        
        return true;
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		int code=0;
		for (Iterator iter = path.iterator(); iter.hasNext();) {
			GraphPathNode node = (GraphPathNode) iter.next();
			code += node.children.length + node.selectedChild + 1;
		}
		return code;
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
        // FIXME: HACK: Got rid of IndexOutOfBoundsExceptiobn
        return "Object Manager";
//		StringBuffer result = new StringBuffer();
//		for (Iterator i = path.iterator(); i.hasNext();) {
//			GraphPathNode node = (GraphPathNode) i.next();
//			if (result.length() > 0)
//				result.append("-->");
//			result.append(node.children[node.selectedChild+1].getText());
//		}
//		return result.toString();
	}
}

