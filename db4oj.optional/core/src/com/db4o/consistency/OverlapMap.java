/* Copyright (C) 2011   Versant Inc.   http://www.db4o.com */
package com.db4o.consistency;

import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.*;

class OverlapMap {

	private Set<Pair<SlotDetail, SlotDetail>> _dupes = new HashSet<Pair<SlotDetail,SlotDetail>>();
	private TreeIntObject _slots = null;
	private final BlockConverter _blockConverter;
	
	public OverlapMap(BlockConverter blockConverter) {
		_blockConverter = blockConverter;
	}

	public void add(SlotDetail slot) {
		if(TreeIntObject.find(_slots, new TreeIntObject(slot._slot.address())) != null) {
			_dupes.add(new Pair<SlotDetail, SlotDetail>(byAddress(slot._slot.address()), slot));
		}
		_slots = (TreeIntObject) TreeIntObject.add(_slots, new TreeIntObject(slot._slot.address(), slot));
	}
	
	public Set<Pair<SlotDetail, SlotDetail>> overlaps() {
		final Set<Pair<SlotDetail, SlotDetail>> overlaps = new HashSet<Pair<SlotDetail, SlotDetail>>();
		final ByRef<SlotDetail> prevSlot = ByRef.newInstance();
		TreeIntObject.traverse(_slots, new Visitor4<TreeIntObject>() {
			public void visit(TreeIntObject tree) {
				SlotDetail curSlot = (SlotDetail) tree._object;
				if(isOverlap(prevSlot.value, curSlot)) {
					overlaps.add(new Pair<SlotDetail, SlotDetail>(prevSlot.value, curSlot));
				}
				prevSlot.value = curSlot;
			}

			private boolean isOverlap(SlotDetail prevSlot, SlotDetail curSlot) {
				if(prevSlot == null){
					return false;
				}
				return prevSlot._slot.address() + _blockConverter.bytesToBlocks(prevSlot._slot.length()) > curSlot._slot.address();
			}
		});
		return overlaps;
	}

	public Set<Pair<SlotDetail, SlotDetail>> dupes() {
		return _dupes;
	}
	
	private SlotDetail byAddress(int address) {
		TreeIntObject tree = (TreeIntObject) TreeIntObject.find(_slots, new TreeIntObject(address, null));
		return tree == null ? null : (SlotDetail)tree._object;
	}
}
