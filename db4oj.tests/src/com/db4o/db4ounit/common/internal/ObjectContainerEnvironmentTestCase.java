package com.db4o.db4ounit.common.internal;

import static com.db4o.foundation.Environments.my;

import com.db4o.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ObjectContainerEnvironmentTestCase extends AbstractDb4oTestCase {
	
	public void testMyObjectContainer() {
		
		container().withEnvironment(new Runnable() { public void run() {
			
			Assert.areSame(container(), my(ObjectContainer.class));
			
		}});
	}

}
