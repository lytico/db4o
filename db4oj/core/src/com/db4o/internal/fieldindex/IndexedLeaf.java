/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.fieldindex;

import com.db4o.foundation.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.query.processor.*;

/**
 * @exclude
 */
public class IndexedLeaf extends IndexedNodeBase implements IndexedNodeWithRange {
	
	private final BTreeRange _range;
    
    public IndexedLeaf(QConObject qcon) {
    	super(qcon);
    	_range = search();
    }
    
    private BTreeRange search() {
		final BTreeRange range = search(constraint().getObject());
        final QEBitmap bitmap = QEBitmap.forQE(constraint().evaluator());
        if (bitmap.takeGreater()) {        
            if (bitmap.takeEqual()) {
                return range.extendToLast();
            }            
            final BTreeRange greater = range.greater();
            if (bitmap.takeSmaller()) {
            	return greater.union(range.smaller());
            }
			return greater;
        }
        if (bitmap.takeSmaller()) {
        	if (bitmap.takeEqual()) {
        		return range.extendToFirst();
        	}
        	return range.smaller();
        }
        return range;
    }

	public int resultSize() {
        return _range.size();
    }

	public Iterator4 iterator() {
		return _range.keys();
	}

	public BTreeRange getRange() {
		return _range;
	}
	
	public void markAsBestIndex(QCandidates candidates) {
		_constraint.setProcessedByIndex(candidates);
	}
	
	public boolean isEmpty(){
		return _range.isEmpty();
	}

}
