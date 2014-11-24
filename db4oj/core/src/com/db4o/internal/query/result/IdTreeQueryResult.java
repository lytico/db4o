/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query.result;

import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public class IdTreeQueryResult extends AbstractQueryResult{
	
	private Tree _ids;

	public IdTreeQueryResult(Transaction transaction, IntIterator4 ids) {
		super(transaction);
		_ids = TreeInt.addAll(null, ids);
	}
	
	public IntIterator4 iterateIDs() {
		return new IntIterator4Adaptor(new TreeKeyIterator(_ids));
	}

	public int size() {
		if(_ids == null){
			return 0;
		}
		return _ids.size();
	}

    public AbstractQueryResult supportSort(){
    	return toIdList();
    }
    
    public AbstractQueryResult supportElementAccess(){
    	return toIdList();
    }
    
}
