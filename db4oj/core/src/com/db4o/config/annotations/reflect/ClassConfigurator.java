/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations.reflect;

import com.db4o.config.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public abstract class ClassConfigurator extends Db4oConfigurator {
	private String _className;
	
	protected ClassConfigurator(String className) {
		_className=className;
	}
	
	@Override
	protected void configure() {
		configure(objectClass(_className));
	}
	
	protected abstract void configure(ObjectClass objectClass);
}
