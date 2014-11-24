/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.freespace;

public class FreespaceVisitor {
    
    int _key;
    int _value;
    
    private boolean _visited;

    public void visit(int key, int value) {
        _key = key;
        _value = value;
        _visited = true;
    }
    
    public boolean visited(){
        return _visited;
    }

}
