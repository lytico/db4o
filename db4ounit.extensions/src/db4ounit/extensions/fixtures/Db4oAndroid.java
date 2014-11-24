/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.fixtures;

import db4ounit.extensions.*;

/**
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class Db4oAndroid extends Db4oSolo{
	
	@Override
	public boolean accept(Class clazz) {
		return !OptOutAndroid.class.isAssignableFrom(clazz) && super.accept(clazz);
	}
	
}
