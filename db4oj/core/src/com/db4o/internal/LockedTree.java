/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;


/**
 * @exclude
 */
public class LockedTree {
    
    private Tree _tree;
    
    private int _version;

    public void add(Tree tree) {
        changed();
        _tree = _tree == null ? tree : _tree.add(tree); 
    }

    private void changed() {
        _version++;
    }

    public void clear() {
        changed();
        _tree = null;
    }

    public Tree find(int key) {
        return TreeInt.find(_tree, key);
    }

    public void read(ByteArrayBuffer buffer, Readable template) {
        clear();
        _tree = new TreeReader(buffer, template).read();
        changed();
    }

    public void traverseLocked(Visitor4 visitor) {
        int currentVersion = _version;
        Tree.traverse(_tree, visitor);
        if(_version != currentVersion){
            throw new IllegalStateException();
        }
    }
    
    public void traverseMutable(Visitor4 visitor){
        final Collection4 currentContent = new Collection4();
        traverseLocked(new Visitor4() {
            public void visit(Object obj) {
                currentContent.add(obj);
            }
        });
        Iterator4 i = currentContent.iterator();
        while(i.moveNext()){
            visitor.visit(i.current());
        }
    }
    
    public boolean isEmpty(){
    	return _tree == null;
    }

}
