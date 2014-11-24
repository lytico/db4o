/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public class DisabledBlockConverter implements BlockConverter {

	public int blockAlignedBytes(int bytes) {
		return bytes;
	}

	public int blocksToBytes(int blocks) {
		return blocks;
	}

	public int bytesToBlocks(long bytes) {
		return (int) bytes;
	}

	public Slot toBlockedLength(Slot slot) {
		return slot;
	}

	public Slot toNonBlockedLength(Slot slot) {
		return slot;
	}

}
