/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.fieldindex;

import com.db4o.foundation.*;
import com.db4o.internal.query.processor.*;

public class FieldIndexProcessor {

	private final QCandidates _candidates;

	public FieldIndexProcessor(QCandidates candidates) {
		_candidates = candidates;
	}
	
	public FieldIndexProcessorResult run() {
		IndexedNode bestIndex = selectBestIndex();
		if (null == bestIndex) {
			return FieldIndexProcessorResult.NO_INDEX_FOUND;
		}
		IndexedNode resolved = resolveFully(bestIndex);
		
		if (! bestIndex.isEmpty()) {
			bestIndex.markAsBestIndex(_candidates);
			return new FieldIndexProcessorResult(resolved);
		}
		return FieldIndexProcessorResult.FOUND_INDEX_BUT_NO_MATCH;
	}

	private IndexedNode resolveFully(IndexedNode indexedNode) {
		if (null == indexedNode) {
			return null;
		}
		if (indexedNode.isResolved()) {
			return indexedNode;
		}
		return resolveFully(indexedNode.resolve());
	}
	
	public IndexedNode selectBestIndex() {		
		final Iterator4 i = collectIndexedNodes();
		IndexedNode best = null;
		while (i.moveNext()) {
			IndexedNode indexedNode = (IndexedNode)i.current();
			IndexedNode resolved = resolveFully(indexedNode);
			if(resolved == null){
				continue;
			}
			if(best == null){
				best = indexedNode;
				continue;
			}
			if (indexedNode.resultSize() < best.resultSize()) {
				best = indexedNode;
			}
		}
		return best;
	}

	public Iterator4 collectIndexedNodes() {
		return new IndexedNodeCollector(_candidates).getNodes();
	}	    
}
