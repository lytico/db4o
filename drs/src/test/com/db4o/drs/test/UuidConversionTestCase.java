/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test;

import com.db4o.drs.foundation.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.data.*;

import db4ounit.*;

public class UuidConversionTestCase extends DrsTestCase{
	
	
	public void test() {
		SPCChild child = storeInA();
		replicate();
		ReplicationReference ref = a().provider().produceReference(child);
		b().provider().clearAllReferences();
		DrsUUID expectedUuid = ref.uuid();
		ReplicationReference referenceByUUID = b().provider().produceReferenceByUUID(expectedUuid, null);
		Assert.isNotNull(referenceByUUID);
		DrsUUID actualUuid = referenceByUUID.uuid();
		Assert.areEqual(expectedUuid.getLongPart(), actualUuid.getLongPart());
	}

	private SPCChild storeInA() {
		String name = "c1";
		SPCChild child = createChildObject(name);
		
		a().provider().storeNew(child);
		a().provider().commit();
		return child;
	}
	
	private void replicate() {
		replicateAll(a().provider(), b().provider());
	}
	
	protected SPCChild createChildObject(String name) {
		return new SPCChild(name);
	}



}
