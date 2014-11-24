/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.query.result.*;

/**
 * @exclude
 */
public class ClientQueryResult extends IdListQueryResult {

	public ClientQueryResult(Transaction ta) {
		super(ta);
	}
    
    public ClientQueryResult(Transaction ta, int initialSize) {
        super(ta, initialSize);
    }
    
    public Iterator4 iterator() {
    	return skip(ClientServerPlatform.createClientQueryResultIterator(this));
    }
}
