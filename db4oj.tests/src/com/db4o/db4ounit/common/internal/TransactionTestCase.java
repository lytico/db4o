/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.internal;

import com.db4o.internal.*;
import com.db4o.internal.references.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;


public class TransactionTestCase extends AbstractDb4oTestCase implements OptOutMultiSession{
    
    private static final int TEST_ID = 5;

    public void testRemoveReferenceSystemOnClose(){
        LocalObjectContainer container = (LocalObjectContainer) db();
        ReferenceSystem referenceSystem = container.createReferenceSystem();
        Transaction transaction = container.newTransaction(container.systemTransaction(), referenceSystem, false);
        
        referenceSystem.addNewReference(new ObjectReference(TEST_ID));
        referenceSystem.addNewReference(new ObjectReference(TEST_ID + 1));
        
        container.referenceSystemRegistry().removeId(TEST_ID);
        Assert.isNull(referenceSystem.referenceForId(TEST_ID));
        
        transaction.close(false);
        
        container.referenceSystemRegistry().removeId(TEST_ID + 1);
        Assert.isNotNull(referenceSystem.referenceForId(TEST_ID + 1));
    }

}
