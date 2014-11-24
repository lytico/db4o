/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/** 
 * marker interface for special db4o datatypes
 */
interface Db4oTypeImpl extends TransactionAware {
	
	int adjustReadDepth(int a_depth);
    
    boolean canBind();
	
	Object createDefault(Transaction a_trans);
	
	boolean hasClassIndex();
    
    void replicateFrom(Object obj);
	
	void setYapObject(YapObject a_yapObject);
	
	Object storedTo(Transaction a_trans);
	
	void preDeactivate();
	
}
