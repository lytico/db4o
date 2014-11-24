/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations.reflect;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class PersistedStaticFieldValuesConfigurator extends Db4oConfigurator {
	private String _className;

	public PersistedStaticFieldValuesConfigurator(String name) {
		_className = name;
	}

	@Override
	protected void configure() {
		objectClass(_className).persistStaticFieldValues();
	}

}
