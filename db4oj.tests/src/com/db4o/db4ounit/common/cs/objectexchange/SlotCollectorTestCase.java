package com.db4o.db4ounit.common.cs.objectexchange;

import static org.easymock.EasyMock.createControl;
import static org.easymock.EasyMock.expect;

import java.util.*;

import org.easymock.*;

import com.db4o.cs.internal.objectexchange.*;
import com.db4o.foundation.*;
import com.db4o.internal.slots.*;

import db4ounit.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class SlotCollectorTestCase implements TestCase {

	final IMocksControl mockery = createControl();
	final ReferenceCollector referenceCollector = mockery.createMock(ReferenceCollector.class);
	final SlotAccessor slotAccessor = mockery.createMock(SlotAccessor.class);
	
	public void testCycles() {
		
		// 1 -> 2 -> 3
		// 2 -> 3 -> 1
		// 3 -> 1 -> 2
		expectReferencesFrom(1, 2);
		expectReferencesFrom(2, 3);
		expectReferencesFrom(3, 1);
		
		final Slot slot1 = expectSlotAccessFor(1);
		final Slot slot2 = expectSlotAccessFor(2);
		final Slot slot3 = expectSlotAccessFor(3);
		
		mockery.replay();
		
		final List<Pair<Integer, Slot>> slots = new SlotCollector(
			3,
			referenceCollector,
			slotAccessor).collect(Iterators.iterate(1, 2));
		
		assertSlots(slots, 
				Pair.of(1, slot1),
				Pair.of(2, slot2),
				Pair.of(3, slot3));
		
		mockery.verify();
	}
	
	public void testDepth3() {
		
		// roots: 1, 2, 3
		// references: 1 -> 4 -> 5 -> *
		// references: 2 -> 6 -> 7 -> *
		// references: 3 -> 9 -> (5, 2) -> *
		
		expectReferencesFrom(1, 4);
		expectReferencesFrom(2, 6);
		expectReferencesFrom(3, 9);
		expectReferencesFrom(4, 5);
		expectReferencesFrom(6, 7);
		expectReferencesFrom(9, 5, 2);
		
		final Slot slot1 = expectSlotAccessFor(1);
		final Slot slot2 = expectSlotAccessFor(2);
		final Slot slot3 = expectSlotAccessFor(3);
		final Slot slot4 = expectSlotAccessFor(4);
		final Slot slot5 = expectSlotAccessFor(5);
		final Slot slot6 = expectSlotAccessFor(6);
		final Slot slot7 = expectSlotAccessFor(7);
		final Slot slot9 = expectSlotAccessFor(9);
		
		mockery.replay();
		
		final List<Pair<Integer, Slot>> slots = new SlotCollector(
			3,
			referenceCollector,
			slotAccessor).collect(Iterators.iterate(1, 2, 3));
		
		assertSlots(slots, 
			Pair.of(1, slot1),
			Pair.of(2, slot2),
			Pair.of(3, slot3),
			Pair.of(4, slot4),
			Pair.of(5, slot5),
			Pair.of(6, slot6),
			Pair.of(7, slot7),
			Pair.of(9, slot9));
		
		
		mockery.verify();
	}

	private void assertSlots(List<Pair<Integer, Slot>> slots, Pair<Integer, Slot>... expected) {
		Iterator4Assert.sameContent(
				Iterators.iterate(expected),
				Iterators.iterator(slots));
	    
    }

	private Slot expectSlotAccessFor(final int id) {
	    final Slot slot = new Slot(id, 0);
		expect(slotAccessor.currentSlotOfID(id))
			.andReturn(slot);
	    return slot;
    }

	private void expectReferencesFrom(final int root, final Integer... children) {
	    expect(referenceCollector.referencesFrom(root))
			.andReturn(Iterators.iterate(children));
    }

}
