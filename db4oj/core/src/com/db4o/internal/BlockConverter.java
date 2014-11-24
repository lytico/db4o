/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public interface BlockConverter {

	public int bytesToBlocks(long bytes);

	public int blockAlignedBytes(int bytes);

	public int blocksToBytes(int blocks);

	public Slot toBlockedLength(Slot slot);

	public Slot toNonBlockedLength(Slot slot);

}