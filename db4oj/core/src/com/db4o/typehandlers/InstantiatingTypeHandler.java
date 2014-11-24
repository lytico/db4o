/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
package com.db4o.typehandlers;

import com.db4o.marshall.*;

public interface InstantiatingTypeHandler extends ReferenceTypeHandler {

	Object instantiate(ReadContext context);
	
	/**
	 * gets called when an object is to be written to the database.
	 * 
	 * The method must only write data necessary to re instantiate the object, usually
	 * the immutable bits of information held by the object. For value
	 * types that means their complete state.
	 * 
	 * Mutable state (only allowed in reference types) must be handled
	 * during {@link ReferenceTypeHandler#activate(WriteContext)}
	 *  
	 * @param context
	 * @param obj the object
	 */
    void writeInstantiation(WriteContext context, Object obj);

}
