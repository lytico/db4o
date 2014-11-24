/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package db4ounit.extensions;

import java.lang.reflect.*;

import com.db4o.io.*;

/**
 * Platform dependent code goes here.
 *
 * @sharpen.ignore
 */
public class Db4oUnitPlatform {

	public static boolean isUserField(Field a_field) {
	    return (!Modifier.isStatic(a_field.getModifiers()))
	        && (!Modifier.isTransient(a_field.getModifiers())
	            & !(a_field.getName().indexOf("$") > -1));
	}

	public static boolean isPascalCase() {
		return false;
	}

	public static Storage newPersistentStorage() {
		return new FileStorage();
	}
}
