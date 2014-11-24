/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;


/**
 * @exclude
 */
public class TransactionContext {
    
    public final Transaction _transaction;
    
    public final Object _object;
    
    public TransactionContext(Transaction transaction, Object obj){
        _transaction = transaction;
        _object = obj;
    }

}
