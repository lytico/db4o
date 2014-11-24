/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations.reflect;

import com.db4o.config.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class IndexedConfigurator extends FieldConfigurator {

	public IndexedConfigurator(String className, String fieldName) {
		super(className, fieldName);
	}

	@Override
	protected void configure(ObjectField objectField) {
		objectField.indexed(true);

	}

}
