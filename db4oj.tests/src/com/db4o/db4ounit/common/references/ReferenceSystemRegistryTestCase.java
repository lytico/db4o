/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.references;

import com.db4o.internal.*;
import com.db4o.internal.references.*;

import db4ounit.*;


public class ReferenceSystemRegistryTestCase implements TestLifeCycle {

    private ReferenceSystemRegistry _registry;
    private ReferenceSystem _referenceSystem1;
    private ReferenceSystem _referenceSystem2;
    
    private static int TEST_ID = 5;

    public void setUp() throws Exception {
        _registry = new ReferenceSystemRegistry();
        _referenceSystem1 = new TransactionalReferenceSystem();
        _referenceSystem2 = new TransactionalReferenceSystem();
        _registry.addReferenceSystem(_referenceSystem1);
        _registry.addReferenceSystem(_referenceSystem2);
    }
    
    public void tearDown() throws Exception {
        
    }
    
    public void testRemoveId(){
        addTestReferenceToBothSystems();
        _registry.removeId(TEST_ID);
        assertTestReferenceNotPresent();
    }

    public void testRemoveNull(){
        _registry.removeObject(null);
    }

    public void testRemoveObject(){
        ObjectReference testReference = addTestReferenceToBothSystems();
        _registry.removeObject(testReference.getObject());
        assertTestReferenceNotPresent();
    }
    
    public void testRemoveReference(){
        ObjectReference testReference = addTestReferenceToBothSystems();
        _registry.removeReference(testReference);
        assertTestReferenceNotPresent();
    }
    
    public void testRemoveReferenceSystem(){
        addTestReferenceToBothSystems();
        _registry.removeReferenceSystem(_referenceSystem1);
        _registry.removeId(TEST_ID);
        Assert.isNotNull(_referenceSystem1.referenceForId(TEST_ID));
        Assert.isNull(_referenceSystem2.referenceForId(TEST_ID));
    }
    
    public void testRemoveByObjectReference(){
    	ObjectReference ref1 = newObjectReference();
    	_referenceSystem1.addExistingReference(ref1);
    	ObjectReference ref2 = newObjectReference();
    	_referenceSystem2.addExistingReference(ref2);
    	_registry.removeReference(ref2);
        Assert.isNotNull(_referenceSystem1.referenceForId(TEST_ID));
        Assert.isNull(_referenceSystem2.referenceForId(TEST_ID));
    }

    private void assertTestReferenceNotPresent() {
        Assert.isNull(_referenceSystem1.referenceForId(TEST_ID));
        Assert.isNull(_referenceSystem2.referenceForId(TEST_ID));
    }

    private ObjectReference addTestReferenceToBothSystems() {
        ObjectReference ref = newObjectReference();
        _referenceSystem1.addExistingReference(ref);
        _referenceSystem2.addExistingReference(ref);
        return ref;
    }

	private ObjectReference newObjectReference() {
		ObjectReference ref = new ObjectReference(TEST_ID);
        ref.setObject(new Object());
		return ref;
	}
    
    

}
