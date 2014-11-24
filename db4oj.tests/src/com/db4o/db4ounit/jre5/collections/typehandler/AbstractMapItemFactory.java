package com.db4o.db4ounit.jre5.collections.typehandler;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public abstract class AbstractMapItemFactory extends AbstractItemFactory {

	public String fieldName() {
		return AbstractItemFactory.MAP_FIELD_NAME;
	}
}
