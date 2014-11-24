/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.handlers;


import db4ounit.*;
import db4ounit.extensions.*;

public class TypeHandlerTestCaseBase extends AbstractDb4oTestCase {
	
	protected void doTestStoreObject(Object storedItem){
        db().store(storedItem);
        db().purge(storedItem);
    
        Object readItem = retrieveOnlyInstance(storedItem.getClass());
        
        Assert.areNotSame(storedItem, readItem);
        Assert.areEqual(storedItem, readItem);
		
	}

}