/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.btree;

/**
 * @exclude
 */
import com.db4o.foundation.*;
import com.db4o.internal.*;

public abstract class BTreeUpdate extends BTreePatch {

	protected BTreeUpdate _next;

	public BTreeUpdate(Transaction transaction, Object obj) {
		super(transaction, obj);
	}

	protected boolean hasNext() {
		return _next != null;
	}

	public BTreePatch forTransaction(Transaction trans) {
	    if(_transaction == trans){
	        return this;
	    }
	    if(_next == null){
	        return null;
	    }
	    return _next.forTransaction(trans);
	}

	public BTreeUpdate removeFor(Transaction trans) {
		if (_transaction == trans) {
			return _next;
		}
		if (_next != null) {
			_next = _next.removeFor(trans);
		}
		return this;
	}

	public void append(BTreeUpdate patch) {
	    if(_transaction == patch._transaction){
	        throw new IllegalArgumentException();
	    }
	    if(!hasNext()){
	        _next = patch;
	    }else{
	        _next.append(patch);
	    }
	}
	
    protected void applyKeyChange(Object obj) {
        _object = obj;
        if (hasNext()) {
            _next.applyKeyChange(obj);      
        }
    }

	protected abstract void committed(BTree btree);

	public Object commit(Transaction trans, BTree btree, BTreeNode node) {
		final BTreeUpdate patch = (BTreeUpdate) forTransaction(trans);
		if (patch instanceof BTreeCancelledRemoval) {
			Object obj = patch.getCommittedObject();
			applyKeyChange(obj);
		} else if (patch instanceof BTreeRemove){
		    removedBy(trans, btree, node);
		    patch.committed(btree);
		    return No4.INSTANCE;
		}
	    return internalCommit(trans, btree);
	}

	protected final Object internalCommit(Transaction trans, BTree btree) {
		if(_transaction == trans){	        
	        committed(btree);
	        if (hasNext()){
	            return _next;
	        }
	        return getCommittedObject();
	    }
	    if(hasNext()){
	        setNextIfPatch(_next.internalCommit(trans, btree));
	    }
	    return this;
	}

	private void setNextIfPatch(Object newNext) {
		if(newNext instanceof BTreeUpdate){
			_next = (BTreeUpdate)newNext;
		} else {
		    _next = null;
		}
	}

	protected abstract Object getCommittedObject();

	public Object rollback(Transaction trans, BTree btree) {
	    if(_transaction == trans){
	        if(hasNext()){
	            return _next;
	        }
	        return getObject();
	    }
	    if(hasNext()){
	        setNextIfPatch(_next.rollback(trans, btree));
	    }
	    return this;
	}
	
	public Object key(Transaction trans) {
		BTreePatch patch = forTransaction(trans);
		if (patch == null) {
			return getObject();
		}
		if (patch.isRemove()) {
			return No4.INSTANCE;
		}
		return patch.getObject();
	}
	
	public BTreeUpdate replacePatch(BTreePatch patch, BTreeUpdate update) {
		if(patch == this){
			update._next = _next;
			return update;
		}
		if(_next == null){
			throw new IllegalStateException();
		}
		_next = _next.replacePatch(patch, update);
		return this;
	}
	
    public void removedBy(Transaction trans, BTree btree, BTreeNode node) {
        if(trans != _transaction){
            adjustSizeOnRemovalByOtherTransaction(btree, node);
        }
        if(hasNext()){
            _next.removedBy(trans, btree, node);
        }
    }
    
    protected abstract void adjustSizeOnRemovalByOtherTransaction(BTree btree, BTreeNode node);
    
	public int sizeDiff(Transaction trans) {
		BTreeUpdate patchForTransaction = (BTreeUpdate) forTransaction(trans);
		if(patchForTransaction == null){
			return 1;
		}
		return patchForTransaction.sizeDiff();
	}

	protected abstract int sizeDiff();

}