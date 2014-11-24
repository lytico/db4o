package com.db4o.db4ounit.jre5.collections.typehandler;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public abstract class AbstractListItemFactory extends AbstractItemFactory {

	public String fieldName() {
		return AbstractItemFactory.LIST_FIELD_NAME;
	}

}
