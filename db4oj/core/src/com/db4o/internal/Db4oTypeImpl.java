/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.*;


/** 
 * marker interface for special db4o datatypes
 * 
 * @exclude
 */
public interface Db4oTypeImpl extends TransactionAware {
	
	Object createDefault(Transaction trans);
	
	boolean hasClassIndex();
	
	void setObjectReference(ObjectReference ref);
	
}
