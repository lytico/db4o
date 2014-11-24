/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */
package com.db4o.inside.btree;

import com.db4o.*;


public class BTreeAdd extends BTreePatch{

    public BTreeAdd(Transaction transaction, Object obj) {
        super(transaction, obj);
    }

    public Object getObject(Transaction trans) {
        if(trans == _transaction){
            return _object;
        }
        return super.getObject(trans);
    }

    
    

}
