package com.db4o.osgi.test;

import org.osgi.framework.BundleActivator;
import org.osgi.framework.BundleContext;

public class Db4oWorkspaceActivator implements BundleActivator {

	private static final String FILENAME = "osgi_test.db4o";
	
	public void start(BundleContext context) throws Exception {
		System.exit(new Db4oTestServiceImpl(context).runTests(FILENAME));

	}

	public void stop(BundleContext context) throws Exception {
		

	}

}
