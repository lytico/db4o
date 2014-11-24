/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations.reflect;

import com.db4o.config.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public abstract class Db4oConfigurator {	
	private Configuration _config;
	private ObjectClass _objectClass;

	public final ObjectClass configure(Configuration config) {
		_config=config;
		configure();
		return _objectClass;
	}
	
	protected abstract void configure();

	protected ObjectClass objectClass(String className) {
		if (_objectClass == null) {
			_objectClass = _config.objectClass(className);
		}
		return _objectClass;
	}
}
