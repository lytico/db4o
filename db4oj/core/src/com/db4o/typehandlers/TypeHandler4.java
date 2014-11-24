/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.typehandlers;

import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.internal.delete.*;
import com.db4o.marshall.*;


/**
 * handles reading, writing, deleting, defragmenting and 
 * comparisons for types of objects.<br><br>
 * Custom Typehandlers can be implemented to alter the default 
 * behaviour of storing all non-transient fields of an object.<br><br>
 * @see {@link Configuration#registerTypeHandler(TypeHandlerPredicate, TypeHandler4)} 
 */
public interface TypeHandler4 {
	
	/**
	 * gets called when an object gets deleted.
	 * @param context 
	 * @throws Db4oIOException
	 */
	void delete(DeleteContext context) throws Db4oIOException;
	
	/**
	 * gets called when an object gets defragmented.
	 * @param context
	 */
	void defragment(DefragmentContext context);
	
	/**
	 * gets called when an object is to be written to the database.

	 * @param context
	 * @param obj the object
	 */
    void write(WriteContext context, Object obj);
	
}
