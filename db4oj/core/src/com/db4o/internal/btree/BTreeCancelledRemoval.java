/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.btree;

import com.db4o.internal.*;

/**
 * @exclude
 */
public class BTreeCancelledRemoval extends BTreeUpdate {
    
    private final Object _newKey;
    
	public BTreeCancelledRemoval(Transaction transaction, Object originalKey, Object newKey, BTreeUpdate existingPatches) {
		super(transaction, originalKey);
		_newKey = newKey;
		if (null != existingPatches) {
			append(existingPatches);
		}
	}
	
	protected void committed(BTree btree) {
	    // do nothing
	}
	
	public boolean isCancelledRemoval() {
		return true;
	}
    
    public String toString() {
        return "(u) " + super.toString();
    }

	protected Object getCommittedObject() {
		return _newKey;
	}

    protected void adjustSizeOnRemovalByOtherTransaction(BTree btree, BTreeNode node) {
        // The other transaction reduces the size, this entry ignores.
    }

	protected int sizeDiff() {
		return 1;
	}
	
}
