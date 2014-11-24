/* Copyright (C) 2010  Versant Corp.  http://www.db4o.com */

package com.db4o.config;

import com.db4o.internal.*;

/**
 * <p>Configures db4objects to work in Dalvik Virtual Machine.
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AndroidSupport implements ConfigurationItem{

	public void apply(InternalObjectContainer container) {
		
	}

	public void prepare(Configuration configuration) {
		configuration.maxStackDepth(2);
	}

}
