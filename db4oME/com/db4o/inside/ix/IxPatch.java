/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.inside.ix;

import com.db4o.*;
import com.db4o.foundation.*;

/**
 * Node for index tree, can be addition or removal node
 */
public abstract class IxPatch extends IxTree {

    int    _parentID;

    Object _value;

    private Queue4 _queue;    // queue of patch objects for the same parent

    IxPatch(IndexTransaction a_ft, int a_parentID, Object a_value) {
        super(a_ft);
        _parentID = a_parentID;
        _value = a_value;
    }

    public Tree add(final Tree a_new) {
        int cmp = compare(a_new);
        if (cmp == 0) {
            IxPatch patch = (IxPatch) a_new;
            cmp = _parentID - patch._parentID;

            if (cmp == 0) {

                Queue4 queue = _queue;

                if (queue == null) {
                    queue = new Queue4();
                    queue.add(this);
                }

                queue.add(patch);
                patch._queue = queue;
                patch._subsequent = _subsequent;
                patch._preceding = _preceding;
                patch.calculateSize();
                return patch;
            }
        }
        return add(a_new, cmp);
    }

    public int compare(Tree a_to) {
        Indexable4 handler = _fieldTransaction.i_index._handler;
        return handler.compareTo(handler.comparableObject(trans(), _value));
    }
    
    public boolean hasQueue() {
		return _queue != null;
	}

	public Queue4 detachQueue() {
		Queue4 queue = _queue;
        this._queue = null;
        return queue;
	}

    protected Tree shallowCloneInternal(Tree tree) {
    	IxPatch patch=(IxPatch)super.shallowCloneInternal(tree);
    	patch._parentID=_parentID;
    	patch._value=_value;
    	patch._queue=_queue;
    	return patch;
    }
}
