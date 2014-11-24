/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre5.collections.typehandler;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public abstract class AbstractItemFactory {
	
	static final String MAP_FIELD_NAME = "_map";
	static final String LIST_FIELD_NAME = "_list";
	
	public abstract String fieldName();
	public abstract Object newItem();
	public abstract Class itemClass();
	public abstract Class containerClass();
}
