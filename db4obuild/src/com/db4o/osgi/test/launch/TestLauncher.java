/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2009  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
package com.db4o.osgi.test.launch;

import java.io.*;
import java.lang.reflect.*;

import org.eclipse.core.runtime.adaptor.EclipseStarter;
import org.osgi.framework.*;

public class TestLauncher {

	private static final int ERROR_EXIT_VALUE = -1;
	private static final String RUNMETHODNAME = "runTests";
	private static final String SERVICENAME = "com.db4o.osgi.test.Db4oTestService";
	private static final String FILENAME = "osgi_test.db4o";

	public static void main(String[] args) throws Exception {
		if(args == null || args.length != 2) {
			System.err.println("Usage: <core bundle path> <test bundle path>");
			System.exit(ERROR_EXIT_VALUE);
		}
		String[] installBundleList = args;
		BundleContext context = setUp(installBundleList, true);
		int exitValue = ERROR_EXIT_VALUE;
		try {
			exitValue = runTests(context);
		}
		finally {
			tearDown();
		}
		System.out.println("EXIT VALUE: " + exitValue);
		System.exit(exitValue);
	}

	public static BundleContext setUp(String[] installBundlePathList, boolean debug) throws Exception {

		System.setProperty("osgi.clean", "true");
		System.setProperty("osgi.parentClassloader", "app");

		EclipseStarter.debug = debug;
		if(EclipseStarter.isRunning()) {
			throw new IllegalStateException("Application already running.");
		}

		BundleContext context = EclipseStarter.startup(new String[] {}, null);
		if(!EclipseStarter.isRunning()) {
			throw new IllegalStateException("Could not start application.");
		}

		System.out.println("STARTING");
		for (int bundlePathIdx = 0; bundlePathIdx < installBundlePathList.length; bundlePathIdx++) {
			String bundlePath = installBundlePathList[bundlePathIdx];
			System.out.println(bundlePath);
			String bundleURL = new File(bundlePath).toURI().toURL().toString();
			Bundle bundle = context.installBundle(bundleURL);
			bundle.start();
		}
		return context;
	}

	private static int runTests(BundleContext context)
			throws NoSuchMethodException, IllegalAccessException,
			InvocationTargetException {
		ServiceReference sRef = context.getServiceReference(SERVICENAME);
		System.out.println("REFERENCE is " + sRef);
	    Object dbs = context.getService(sRef);
		System.out.println("SERVICE is " + dbs);
		Method runMethod = dbs.getClass().getMethod(RUNMETHODNAME, new Class[]{String.class});
		runMethod.setAccessible(true);
		Integer numFailures = (Integer) runMethod.invoke(dbs, new Object[]{FILENAME});
		return numFailures.intValue();
	}

	public static void tearDown() throws Exception {
		// stop eclipse
		EclipseStarter.shutdown();
		if(EclipseStarter.isRunning()) {
			throw new IllegalStateException("Could not stop application.");
		}
	}

}
