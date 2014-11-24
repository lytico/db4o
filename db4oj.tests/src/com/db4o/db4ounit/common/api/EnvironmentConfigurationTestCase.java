/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.api;

import static com.db4o.foundation.Environments.my;

import com.db4o.config.*;
import com.db4o.internal.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class EnvironmentConfigurationTestCase extends AbstractInMemoryDb4oTestCase {
	
	public void test() {
		container().withEnvironment(new Runnable() { public void run() {
			
			Assert.areSame(_service, my(ServiceInterface.class));
			
		}});
	}
	
	public interface ServiceInterface {
	}
	
	private ServiceInterface _service = new ServiceInterface() {};
	
	@Override
	protected void configure(Configuration legacy) throws Exception {
		super.configure(legacy);
		
		final CommonConfiguration common = Db4oLegacyConfigurationBridge.asCommonConfiguration(legacy);
		common.environment().add(_service);
	}

}
