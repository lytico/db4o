/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */
package com.db4o.internal.btree;

import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public class BTreeRemove extends BTreeUpdate {
	
	public BTreeRemove(Transaction transaction, Object obj) {
        super(transaction, obj);
    }
    
    protected void committed(BTree btree){
        btree.notifyRemoveListener(new TransactionContext(_transaction, getObject()));
    }
    
    public String toString() {
        return "(-) " + super.toString();
    }
    
    public boolean isRemove() {
        return true;
    }

	protected Object getCommittedObject() {
		return No4.INSTANCE;
	}

    protected void adjustSizeOnRemovalByOtherTransaction(BTree btree, BTreeNode node) {
        // The size was reduced for this entry, let's change back.
        btree.sizeChanged(_transaction, node, +1);
    }
    
	protected int sizeDiff() {
		return 0;
	}
    
}
