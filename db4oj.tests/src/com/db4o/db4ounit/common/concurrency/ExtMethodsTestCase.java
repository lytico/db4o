/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ExtMethodsTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new ExtMethodsTestCase().runConcurrency();
	}
	
	public void conc(ExtObjectContainer oc) {

		ExtMethodsTestCase em = new ExtMethodsTestCase();
		oc.store(em);
		Assert.isFalse(oc.isClosed());

		Assert.isTrue(oc.isActive(em));
		Assert.isTrue(oc.isStored(em));

		oc.deactivate(em, 1);
		Assert.isTrue(!oc.isActive(em));

		oc.activate(em, 1);
		Assert.isTrue(oc.isActive(em));

		long id = oc.getID(em);
		Assert.isTrue(oc.isCached(id));

		oc.purge(em);
		Assert.isFalse(oc.isCached(id));
		Assert.isFalse(oc.isStored(em));
		Assert.isFalse(oc.isActive(em));

		oc.bind(em, id);
		Assert.isTrue(oc.isCached(id));
		Assert.isTrue(oc.isStored(em));
		Assert.isTrue(oc.isActive(em));

		ExtMethodsTestCase em2 = (ExtMethodsTestCase) oc.getByID(id);
		Assert.areSame(em, em2);

		// Purge all and try again
		oc.purge();

		Assert.isTrue(oc.isCached(id));
		Assert.isTrue(oc.isStored(em));
		Assert.isTrue(oc.isActive(em));

		em2 = (ExtMethodsTestCase) oc.getByID(id);
		Assert.areSame(em, em2);

		oc.delete(em2);
		oc.commit();
		Assert.isFalse(oc.isCached(id));
		Assert.isFalse(oc.isStored(em2));
		Assert.isFalse(oc.isActive(em2));

		// Null checks
		Assert.isFalse(oc.isStored(null));
		Assert.isFalse(oc.isActive(null));
		Assert.isFalse(oc.isCached(0));

	}

}
