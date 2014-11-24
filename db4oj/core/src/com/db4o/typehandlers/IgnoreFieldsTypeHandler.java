/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.typehandlers;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.delete.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;

/**
 * Typehandler that ignores all fields on a class
 */
public class IgnoreFieldsTypeHandler implements ReferenceTypeHandler, CascadingTypeHandler{
	
	public static final TypeHandler4 INSTANCE = new IgnoreFieldsTypeHandler();
	
	private IgnoreFieldsTypeHandler() {
	}

	public void defragment(DefragmentContext context) {
		// do nothing
	}

	public void delete(DeleteContext context) throws Db4oIOException {
		// do nothing
	}

	public void activate(ReferenceActivationContext context) {
	}

	public void write(WriteContext context, Object obj) {
	}

	public PreparedComparison prepareComparison(Context context, Object obj) {
		return null;
	}

	public void cascadeActivation(ActivationContext context) {
		// do nothing
	}

	public void collectIDs(QueryingReadContext context) {
		// do nothing
	}

	public TypeHandler4 readCandidateHandler(QueryingReadContext context) {
		return null;
	}

}
