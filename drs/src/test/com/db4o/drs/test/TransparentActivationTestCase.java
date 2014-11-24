/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.drs.test;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.test.data.*;
import com.db4o.ta.*;

import db4ounit.*;

public class TransparentActivationTestCase extends DrsTestCase{
	
	@Override
	protected void configure(Configuration config) {
		config.add(new TransparentActivationSupport());
	}
	
	public void test() throws Exception{
		ActivatableItem item = new ActivatableItem("foo");
		a().provider().storeNew(item);
		a().provider().commit();
		
		if(a().provider() instanceof Db4oReplicationProvider){
			// TODO: We can't reopen Hibernate providers here if
			// they run on an in-memory database.
			
			// db4o should be reopened, otherwise Transparent Activation
			// is not tested.
			
			reopen();
		}
		
		replicateAll(a().provider(), b().provider());
		System.gc(); // Improve chances TA is really required
		ObjectSet items = b().provider().getStoredObjects(ActivatableItem.class);
		Assert.isTrue(items.hasNext());
		ActivatableItem replicatedItem = (ActivatableItem) items.next();
		Assert.areEqual(item.name(), replicatedItem.name());		
	}

}
