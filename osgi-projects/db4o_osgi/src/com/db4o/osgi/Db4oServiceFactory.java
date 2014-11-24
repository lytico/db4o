/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.osgi;

import org.osgi.framework.*;

class Db4oServiceFactory implements ServiceFactory {

	public Object getService(Bundle bundle, ServiceRegistration registration) {
		return new Db4oServiceImpl(bundle);
	}

	public void ungetService(Bundle bundle, ServiceRegistration registration, Object service) {
	}

}
