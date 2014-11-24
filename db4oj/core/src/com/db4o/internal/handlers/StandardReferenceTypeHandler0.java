/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers;

import com.db4o.internal.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;


/**
 * @exclude
 */
public class StandardReferenceTypeHandler0 extends StandardReferenceTypeHandler{

	@Override
	protected MarshallingInfo ensureFieldList(final MarshallingInfo context) {
		return new MarshallingInfo() {
		
			public void declaredAspectCount(int count) {
				context.declaredAspectCount(count);
			}
		
			public int declaredAspectCount() {
				return context.declaredAspectCount();
			}
		
			public boolean isNull(int fieldIndex) {
				return false;
			}
		
			public ClassMetadata classMetadata() {
				return context.classMetadata();
			}
		
			public ReadBuffer buffer() {
				return context.buffer();
			}
		
			public void beginSlot() {
				context.beginSlot();
			}
		};
    }

}
