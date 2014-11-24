/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */
package com.db4o.inside.btree;

import com.db4o.*;


public abstract class BTreePatch {
    
    private BTreePatch _previous;
    
    private BTreePatch _next;
    
    final Transaction _transaction;
    
    final Object _object;

    public BTreePatch(Transaction transaction, Object obj) {
        _transaction = transaction;
        _object = obj;
    }
    
    public BTreePatch forTransaction(Transaction trans){
        if(_transaction == trans){
            return this;
        }
        if(_next == null){
            return null;
        }
        return _next.forTransaction(trans);
    }
    
    public Object getObject(Transaction trans){
        BTreePatch patch = forTransaction(trans);
        if(patch == null){
            return Null.INSTANCE;
        }
        return patch.getObject(trans);
    }
    
    
    
    
    

}
