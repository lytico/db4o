/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.freespace;

import com.db4o.internal.*;
import com.db4o.internal.freespace.*;
import com.db4o.internal.slots.*;

import db4ounit.*;

@decaf.Remove(decaf.Platform.JDK11)
public class FreespaceRemainderLimitTestCase implements TestCase {
	
	private static final int BLOCK_SIZE = 7;
	
	private final BlockConverter _blockConverter = new BlockSizeBlockConverter(BLOCK_SIZE);
	
	private final InMemoryFreespaceManager _blocked = new InMemoryFreespaceManager(null, 0, 2);
	
	private final BlockAwareFreespaceManager _nonBlocked = new BlockAwareFreespaceManager(_blocked , new BlockSizeBlockConverter(BLOCK_SIZE));

	public void testAllocateSlotWithRemainder(){
		assertAllocateSlot(3 * BLOCK_SIZE, BLOCK_SIZE + 1, 3 * BLOCK_SIZE);
	}

	public void testAllocateSlotNoRemainder(){
		assertAllocateSlot(5 * BLOCK_SIZE, BLOCK_SIZE + 1, 2 * BLOCK_SIZE);
	}

	private void assertAllocateSlot(int freeSlotSize, int requiredSlotSize, int expectedSlotSize) {
		Slot slot = new Slot(1, freeSlotSize);
		_nonBlocked.free(slot);		
		Slot allocatedSlot = _nonBlocked.allocateSlot(requiredSlotSize);
		int expectedAllocatedSlotLength = _blockConverter.blockAlignedBytes(expectedSlotSize);
		Slot expectedSlot = new Slot(1, expectedAllocatedSlotLength);
		Assert.areEqual(expectedSlot, allocatedSlot);
	}

}
