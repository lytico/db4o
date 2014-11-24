/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.api;

/**
 * A reference to a field.. 
 */
public interface FieldRef {

	/**
	 * @sharpen.property
	 */
	TypeRef type();

	/**
	 * @sharpen.property
	 */
	String name();

}
