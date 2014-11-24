package com.db4o.cs.internal.objectexchange;

import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.slots.*;

public class SlotCollector {
	
	private SlotAccessor _slotAccessor;
	private ReferenceCollector _referenceCollector;
	private int _depth;
	private Map<Integer, Collection4> referenceCache = new HashMap<Integer, Collection4>();
	
	public SlotCollector(int depth, ReferenceCollector collector, SlotAccessor accessor) {
		if (depth < 1) {
			throw new IllegalArgumentException();
		}
	    _depth = depth;
	    _slotAccessor = accessor;
	    _referenceCollector = collector;
    }

	public List<Pair<Integer, Slot>> collect(Iterator4<Integer> roots) {
		return childSlotsFor(roots);
    }

	private List<Pair<Integer, Slot>> childSlotsFor(Iterator4<Integer> slots) {
		
		final ArrayList<Pair<Integer, Slot>> result = new ArrayList<Pair<Integer, Slot>>();
		
		collectSlots(slots, result, _depth);
		
		return result;
    }

	private void collectSlots(Iterator4<Integer> ids, final ArrayList<Pair<Integer, Slot>> result,
            int currentDepth) {
		
		while (ids.moveNext()) {
			final int id = ids.current();
			if (!containsSlotFor(result, id)) {
				result.add(idSlotPairFor(id));
			}
			if (currentDepth > 1) {
				final Iterator4 childIds = collectChildIdsFor(id);
				collectSlots(childIds, result, currentDepth - 1);
			}
        }
    }

	private boolean containsSlotFor(ArrayList<Pair<Integer, Slot>> result, int id) {
		for (Pair<Integer, Slot> pair : result) {
	        if (pair.first == id) {
	        	return true;
	        }
        }
		return false;
    }

	private Iterator4 collectChildIdsFor(final int id) {
		Collection4 references = referenceCache.get(id);
		if (null == references) {
			references = new Collection4(_referenceCollector.referencesFrom(id));
			referenceCache .put(id, references);
		}
		return references.iterator();
    }
	
	private Pair<Integer, Slot> idSlotPairFor(final int id) {
		return Pair.of(id, _slotAccessor.currentSlotOfID(id));
    }
}
