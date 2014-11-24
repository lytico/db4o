/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.ta;

import com.db4o.db4ounit.common.ta.*;

import db4ounit.extensions.*;


public abstract class TAItemTestCaseBase
	extends ItemTestCaseBase
	implements OptOutTA {
    
    public void testGetByID() throws Exception {
    	Object item = db().ext().getByID(id);
    	assertNullItem(item);
        assertItemValue(item);
	}
    
    public void testGetByUUID() throws Exception {
    	Object item = db().ext().getByUUID(uuid);
    	assertNullItem(item);
        assertItemValue(item);
	}
	
    protected void assertRetrievedItem(Object obj) throws Exception {
    	assertNullItem(obj);
    }
}
