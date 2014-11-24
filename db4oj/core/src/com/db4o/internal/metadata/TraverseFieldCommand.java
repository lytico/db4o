/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.internal.metadata;

import com.db4o.internal.*;

/**
 * @exclude
 */
public abstract class TraverseFieldCommand implements TraverseAspectCommand{

	public boolean cancelled() {
		return false;
	}

	public int declaredAspectCount(ClassMetadata classMetadata) {
		return classMetadata.declaredAspectCount();
	}

	public void processAspect(ClassAspect aspect, int currentSlot) {
		if(aspect instanceof FieldMetadata){
			process((FieldMetadata) aspect);
		}
	}

	public void processAspectOnMissingClass(ClassAspect aspect, int currentSlot) {
		// do nothing
	}
	
	protected abstract void process(FieldMetadata field);

}
