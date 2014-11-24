/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.internal.metadata;

import com.db4o.internal.*;

/**
 * @exclude
 */
public class StandardAspectTraversalStrategy implements AspectTraversalStrategy {
	
	private final ClassMetadata _classMetadata;

	public StandardAspectTraversalStrategy(ClassMetadata classMetadata) {
		_classMetadata = classMetadata;
	}

	public void traverseAllAspects(TraverseAspectCommand command) {
		ClassMetadata classMetadata = _classMetadata;
		int currentSlot = 0;
	    while(classMetadata != null){
	        int aspectCount=command.declaredAspectCount(classMetadata);
			for (int i = 0; i < aspectCount && !command.cancelled(); i++) {
			    command.processAspect(classMetadata._aspects[i],currentSlot);
			    currentSlot++;
			}
	        if(command.cancelled()){
	            return;
	        }
	        classMetadata = classMetadata._ancestor;
	    }
	}
	
	
	
	
}