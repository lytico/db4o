/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.references;

import com.db4o.internal.*;
import com.db4o.internal.references.*;

import db4ounit.extensions.*;


public class ReferenceSystemIntegrationTestCase extends AbstractDb4oTestCase{
	
	private static final int[] IDS = new int[] {100,134,689, 666, 775};
	
	private static final Object[] REFERENCES = createReferences();

	public static void main(String[] args) {
		new ReferenceSystemIntegrationTestCase().runSolo();
	}
	
	public void testTransactionalReferenceSystem(){
		ReferenceSystem transactionalReferenceSystem = new TransactionalReferenceSystem();
		assertAllRerefencesAvailableOnNew(transactionalReferenceSystem);
		transactionalReferenceSystem.rollback();
		assertEmpty(transactionalReferenceSystem);
		assertAllRerefencesAvailableOnCommit(transactionalReferenceSystem);
	}
	
	public void testHashCodeReferenceSystem(){
		ReferenceSystem referenceSystem = new HashcodeReferenceSystem();
		assertAllRerefencesAvailableOnNew(referenceSystem);
	}
	
	private void assertAllRerefencesAvailableOnCommit(ReferenceSystem referenceSystem){
		fillReferenceSystem(referenceSystem);
		referenceSystem.commit();
		assertAllReferencesAvailable(referenceSystem);
	}
	
	private void assertAllRerefencesAvailableOnNew(ReferenceSystem referenceSystem){
		fillReferenceSystem(referenceSystem);
		assertAllReferencesAvailable(referenceSystem);
	}
	
	private void assertEmpty(ReferenceSystem referenceSystem){
		assertContains(referenceSystem, new Object[]{});
	}
	
	private void assertAllReferencesAvailable(ReferenceSystem referenceSystem){
		assertContains(referenceSystem, REFERENCES);
	}

	private void assertContains(ReferenceSystem referenceSystem, final Object[] objects) {
		ExpectingVisitor expectingVisitor = new ExpectingVisitor(objects);
		referenceSystem.traverseReferences(expectingVisitor);
		expectingVisitor.assertExpectations();
	}
	
	private void fillReferenceSystem(ReferenceSystem referenceSystem){
		for (int i = 0; i < REFERENCES.length; i++) {
			referenceSystem.addNewReference((ObjectReference)REFERENCES[i]);
		}
	}
	
	private static Object[] createReferences() {
		Object[] references = new Object[IDS.length];
		for (int i = 0; i < IDS.length; i++) {
			ObjectReference ref = new ObjectReference(IDS[i]);
			ref.setObject(new Integer(IDS[i]).toString());
			references[i]= ref; 
		}
		return references;
	}

}
