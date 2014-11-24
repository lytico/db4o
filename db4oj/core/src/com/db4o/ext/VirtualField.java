/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */
package com.db4o.ext;

import com.db4o.internal.*;

/**
 * 
 * intended for future virtual fields on classes. Currently only
 * the constant for the virtual version field is found here.  
 *
 * @exclude
 */
public class VirtualField {
	
	/**
	 * the field name of the virtual version field, to be used
	 * for querying.
	 */
	public static final String VERSION = Const4.VIRTUAL_FIELD_PREFIX + "version"; 
	
	public static final String COMMIT_TIMESTAMP = Const4.VIRTUAL_FIELD_PREFIX + "commitTimestamp"; 

}
