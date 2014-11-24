/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test;

import com.db4o.drs.test.data.*;

import db4ounit.*;

public class ReplicatingTwiceTestCase extends DrsTestCase {
	
	public void test(){
		Pilot pilot = new Pilot("one", 1);
		a().provider().storeNew(pilot);
		a().provider().commit();
		
		replicateAll(a().provider(),b().provider(), null);
		pilot.setName("modified");
		a().provider().update(pilot);
		a().provider().commit();
		replicateAll(a().provider(),b().provider(), null);
		Pilot pilotFromB = (Pilot) getOneInstance(b(), Pilot.class);
		Assert.areEqual("modified", pilotFromB.name());
	}

}
