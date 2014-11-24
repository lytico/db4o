/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.defragment;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.ids.*;
import com.db4o.internal.slots.*;


/**
 * In-memory mapping for IDs during a defragmentation run.
 * This is faster than the {@link DatabaseIdMapping} but
 * it uses more memory. If you have OutOfMemory conditions
 * with this id mapping, use the {@link DatabaseIdMapping}
 * instead.
 * 
 * @see Defragment
 */
public class InMemoryIdMapping extends AbstractIdMapping {
	
	private IdSlotTree _idsToSlots;
	
	private Tree _tree;
	
	public int mappedId(int oldID) {
		int classID = mappedClassID(oldID);
		if(classID != 0) {
			return classID;
		}
		TreeIntObject res = (TreeIntObject) TreeInt.find(_tree, oldID);
		if(res != null){
			return ((Integer)res._object).intValue();
		}
		return 0;
	}

	public void open() {
	}
	
	public void close() {
	}

	protected void mapNonClassIDs(int origID, int mappedID) {
		_tree = Tree.add(_tree, new TreeIntObject(origID, new Integer(mappedID)));
	}
	
	public int addressForId(int id){
		IdSlotTree node = (IdSlotTree) _idsToSlots.find(id);
		if(node == null){
			throw new IllegalStateException();
		}
		return node.slot().address();
	}

	public void mapId(int id, Slot slot) {
		IdSlotTree idSlotMapping = new IdSlotTree(id, slot);
		_idsToSlots = Tree.add(_idsToSlots, idSlotMapping);
	}

	public Visitable<SlotChange> slotChanges() {
		return new Visitable<SlotChange>() {
			public void accept(final Visitor4<SlotChange> outSideVisitor) {
				Tree.traverse(_idsToSlots, new Visitor4<IdSlotTree>() {
					public void visit(IdSlotTree idSlotMapping) {
						SlotChange slotChange = new SlotChange(idSlotMapping._key);
						slotChange.notifySlotCreated(idSlotMapping.slot());
						outSideVisitor.visit(slotChange);
					}
				});
			}
		};
	}

	public void commit() {
	}
}
