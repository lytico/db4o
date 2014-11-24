package com.db4o.internal.marshall;

import com.db4o.internal.*;

public class UnknownTypeHandlerAspect extends FieldMetadata {

	public UnknownTypeHandlerAspect(ClassMetadata containingClass, String name) {
	    super(containingClass, name);
    }
	
	@Override
	public void defragAspect(DefragmentContext context) {
		throw new IllegalStateException("Type handler for '" + containingClass() +
                "' could not be found. Defragment cannot proceed. " + 
				" Please ensure all required types are available and try again.");

	}
	
	@Override
	public boolean alive() {
		return false;
	}

}
