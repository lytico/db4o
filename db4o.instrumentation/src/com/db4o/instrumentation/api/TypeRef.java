/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.api;

public interface TypeRef {

	/**
	 * @sharpen.property
	 */
	boolean isPrimitive();

	/**
	 * @sharpen.property
	 */
	TypeRef elementType();

	/**
	 * @sharpen.property
	 */
	String name();

}
