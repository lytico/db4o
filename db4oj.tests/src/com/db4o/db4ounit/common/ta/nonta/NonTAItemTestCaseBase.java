/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.nonta;

import com.db4o.db4ounit.common.ta.*;

public abstract class NonTAItemTestCaseBase extends ItemTestCaseBase {
	
    protected void assertRetrievedItem(Object obj) {
        //do nothing for non-TA tests
        return;
    }
    
    public void testGetByID() throws Exception {
    	
    	Object item = db().ext().getByID(id);
    	assertNullItem(item);

    	db().activate(item, 15);
        assertItemValue(item);
    	
	}
    
    public void testGetByUUID() throws Exception {
    	Object item = db().ext().getByUUID(uuid);
    	assertNullItem(item);
    	
    	db().activate(item, 15);
        assertItemValue(item);
	}
    
}
