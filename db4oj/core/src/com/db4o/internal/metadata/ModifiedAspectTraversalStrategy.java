/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.internal.metadata;

import java.util.*;

import com.db4o.internal.*;
import com.db4o.internal.metadata.HierarchyAnalyzer.*;

/**
 * @exclude
 */
public class ModifiedAspectTraversalStrategy implements AspectTraversalStrategy {
	
	private final List<Diff> _classDiffs;

	public ModifiedAspectTraversalStrategy(ClassMetadata classMetadata,
			List<Diff> ancestors) {
		_classDiffs = new ArrayList<Diff>();
		_classDiffs.add(new HierarchyAnalyzer.Same(classMetadata));
		_classDiffs.addAll(ancestors);
	}

	public void traverseAllAspects(TraverseAspectCommand command) {
		int currentSlot = 0;
	    for(HierarchyAnalyzer.Diff diff : _classDiffs){
			ClassMetadata classMetadata = diff.classMetadata();
			if(diff.isRemoved()){
		        currentSlot = skipAspectsOf(classMetadata, command,
						currentSlot);
				continue;
			}
	        currentSlot = traverseAspectsOf(classMetadata, command, currentSlot);
	        if(command.cancelled()){
	            return;
	        }
	    }
	}
	
	static interface TraverseAspectCommandProcessor {
		void process(TraverseAspectCommand command, ClassAspect currentAspect, int currentSlot);
	}

	private int traverseAspectsOf(final ClassMetadata classMetadata,
			TraverseAspectCommand command, int currentSlot) {
		return processAspectsOf(classMetadata, command, currentSlot, new TraverseAspectCommandProcessor() {
			public void process(TraverseAspectCommand command, ClassAspect currentAspect, int currentSlot) {
				command.processAspect(currentAspect,currentSlot);
		
			}
		});
	}

	private int processAspectsOf(final ClassMetadata classMetadata,
			TraverseAspectCommand command, int currentSlot,
			TraverseAspectCommandProcessor processor) {
		int aspectCount=command.declaredAspectCount(classMetadata);
		for (int i = 0; i < aspectCount && !command.cancelled(); i++) {
		    processor.process(command, classMetadata._aspects[i], currentSlot);
		    currentSlot++;
		}
		return currentSlot;
	}
	
	private int skipAspectsOf(ClassMetadata classMetadata,
			TraverseAspectCommand command, int currentSlot) {
		return processAspectsOf(classMetadata, command, currentSlot, new TraverseAspectCommandProcessor() {
			public void process(
					TraverseAspectCommand command,
					ClassAspect currentAspect,
					int currentSlot) {
				command.processAspectOnMissingClass(currentAspect, currentSlot);
			}
		});
	}


}
