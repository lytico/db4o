/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public final class BlockSizeBlockConverter implements BlockConverter {
	
	private final int _blockSize;

	public BlockSizeBlockConverter(int blockSize) {
		_blockSize = blockSize;
	}
	
    public int bytesToBlocks(long bytes) {
    	return (int) ((bytes + _blockSize -1 )/ _blockSize);
    }
    
    public int blockAlignedBytes(int bytes) {
    	return bytesToBlocks(bytes) * _blockSize;
    }
    
    public int blocksToBytes(int blocks){
    	return blocks * _blockSize;
    }
    
    public Slot toBlockedLength(Slot slot){
    	return new Slot(slot.address(), bytesToBlocks(slot.length()));
    }
    
    public Slot toNonBlockedLength(Slot slot){
    	return new Slot(slot.address(), blocksToBytes(slot.length()));
    }
    

}
