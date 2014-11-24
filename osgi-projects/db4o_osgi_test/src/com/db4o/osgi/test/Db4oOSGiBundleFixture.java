/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.osgi.test;

import java.io.*;

import org.osgi.framework.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.osgi.*;

import db4ounit.extensions.fixtures.*;
import db4ounit.extensions.util.*;

class Db4oOSGiBundleFixture extends AbstractSoloDb4oFixture {

	private final BundleContext _context;
	private final String _fileName;
	private Configuration _config;
	
	public Db4oOSGiBundleFixture(BundleContext context, String fileName) {
		_context = context;
		_fileName = CrossPlatformServices.databasePath(fileName);
	}
	
	protected Configuration newConfiguration() {
		return service(_context).newConfiguration();
	}

	protected ObjectContainer createDatabase(Configuration config) {
		_config = config;
	    return service(_context).openFile(_config,_fileName);
	}

	private static Db4oService service(BundleContext context) {
		ServiceReference sRef = context.getServiceReference(Db4oService.class.getName());
	    Db4oService dbs = (Db4oService)context.getService(sRef);
		return dbs;
	}

	protected void doClean() {
		_config = null;
		new File(_fileName).delete();
	}

	public void defragment() throws Exception {
		defragment(_fileName);
	}

	public String label() {
		return "OSGi/bundle";
	}

	private final static Class[] OPT_OUT = { OptOutNoFileSystemData.class, OptOutCustomContainerInstantiation.class, OptOutNoInheritedClassPath.class };

	public boolean accept(Class clazz) {
		if(!super.accept(clazz)) {
			return false;
		}
		for (int optOutIdx = 0; optOutIdx < OPT_OUT.length; optOutIdx++) {
			if(OPT_OUT[optOutIdx].isAssignableFrom(clazz)) {
				return false;
			}
		}
		return true;
	}
}
