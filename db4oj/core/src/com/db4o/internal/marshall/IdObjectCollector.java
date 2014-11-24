/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.foundation.*;
import com.db4o.internal.*;


/**
 * @exclude
 */
public class IdObjectCollector {
    
    private TreeInt _ids;
    
    private List4 _objects;
    
    public void addId(int id) {
        _ids = (TreeInt) Tree.add(_ids, new TreeInt(id));
    }
    
    public TreeInt ids() {
        return _ids;
    }
    
    public void add(Object obj) {
        _objects = new List4(_objects, obj);
    }
    
    public Iterator4 objects(){
        return new Iterator4Impl(_objects);
    }

}
