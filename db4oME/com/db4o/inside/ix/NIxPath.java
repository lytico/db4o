/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.ix;

import com.db4o.*;
import com.db4o.foundation.*;

/**
 * @exclude
 */
public class NIxPath extends Tree {
    
    // for debugging purposes, turn on in IxTraverser also
    // public Object _constraint;
    
    final NIxPathNode _head;
    
    final boolean _takePreceding;
    
    final boolean _takeMatches; 
    
    final boolean _takeSubsequent;
    
    int _type;  // see constants in IxTraverser
    
    public NIxPath(NIxPathNode head, boolean takePreceding, boolean takeMatches, boolean takeSubsequent, int pathType){
        _head = head;
        _takePreceding = takePreceding;
        _takeMatches = takeMatches;
        _takeSubsequent = takeSubsequent;
        _type = pathType;
    }
    
    public int compare(Tree a_to) {
        NIxPath other = (NIxPath)a_to;
        return _head.compare(other._head, _type, other._type);
    }
    
    public String toString() {
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
        String str = "NIxPath +\n";
        String space = " ";
        NIxPathNode node = _head;
        while(node != null){
            str += space;
            space += " ";
            str += node.toString() + "\n";
            node = node._next;
        }
        return str;
    }

    public Object shallowClone() {
    	NIxPath path=new NIxPath(_head,_takePreceding,_takeMatches,_takeSubsequent,_type);
    	super.shallowCloneInternal(path);
    	return path;
    }
}
