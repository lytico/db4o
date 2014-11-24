/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */
package com.db4o.internal.btree;

import com.db4o.internal.*;


public abstract class BTreePatch {    
    
    protected final Transaction _transaction;
    
    protected Object _object;

    public BTreePatch(Transaction transaction, Object obj) {
        _transaction = transaction;
        _object = obj;
    }    
    
    public abstract Object commit(Transaction trans, BTree btree, BTreeNode node);

    public abstract BTreePatch forTransaction(Transaction trans);
    
    public Object getObject() {
        return _object;
    }
    
    public boolean isAdd() {
        return false;
    }
    
	public boolean isCancelledRemoval() {
		return false;
	}
	
    public boolean isRemove() {
        return false;
    }

    public abstract Object key(Transaction trans);
    
    public abstract Object rollback(Transaction trans, BTree btree);
    
    public String toString(){
        if(_object == null){
            return "[NULL]";
        }
        return _object.toString();
    }

	public abstract int sizeDiff(Transaction trans);

}
