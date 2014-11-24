/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.header;

import com.db4o.ext.*;
import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class IdentityTestCase extends AbstractDb4oTestCase {

	public static void main(String[] arguments) {
		new IdentityTestCase().runAll();
	}
	
	public void testIdentitySignatureIsNotNull() {
		Db4oDatabase identity = db().identity();
		Assert.isNotNull(identity.getSignature());
	}

	public void testIdentityPreserved() throws Exception {

		Db4oDatabase ident = db().identity();

		reopen();

		Db4oDatabase ident2 = db().identity();

		Assert.isNotNull(ident);
		Assert.areEqual(ident, ident2);
	}

	public void testGenerateIdentity() throws Exception {
		if(isMultiSession()){
			return;
		}

		byte[] oldSignature = db().identity().getSignature();

		generateNewIdentity();

		reopen();

		ArrayAssert.areNotEqual(oldSignature, db().identity().getSignature());
	}

	private void generateNewIdentity() {
		((LocalObjectContainer) db()).generateNewIdentity();
	}
}
