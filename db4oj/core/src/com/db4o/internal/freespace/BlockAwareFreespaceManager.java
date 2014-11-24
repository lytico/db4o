/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.freespace;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public class BlockAwareFreespaceManager implements FreespaceManager {
	
	private final FreespaceManager _delegate;
	
	private final BlockConverter _blockConverter;

	public BlockAwareFreespaceManager(FreespaceManager delegate_, BlockConverter blockConverter) {
		_delegate = delegate_;
		_blockConverter = blockConverter;
	}

	public Slot allocateSlot(int length) {
		Slot slot = _delegate.allocateSlot(_blockConverter.bytesToBlocks(length));
		if(slot == null){
			return null;
		}
		return _blockConverter.toNonBlockedLength(slot);
	}

	public Slot allocateSafeSlot(int length) {
		Slot slot = _delegate.allocateSafeSlot(_blockConverter.bytesToBlocks(length));
		if(slot == null){
			return null;
		}
		return _blockConverter.toNonBlockedLength(slot);
	}

	public void beginCommit() {
		_delegate.beginCommit();
	}

	public void commit() {
		_delegate.commit();
	}

	public void endCommit() {
		_delegate.endCommit();
	}

	public void free(Slot slot) {
		_delegate.free(_blockConverter.toBlockedLength(slot));
	}

	public void freeSelf() {
		_delegate.freeSelf();
	}

	public void freeSafeSlot(Slot slot) {
		_delegate.freeSafeSlot(_blockConverter.toBlockedLength(slot));
		
	}

	public void listener(FreespaceListener listener) {
		_delegate.listener(listener);
	}

	public void migrateTo(FreespaceManager fm) {
		throw new IllegalStateException();
	}

	public int slotCount() {
		return _delegate.slotCount();
	}

	public void start(int id) {
		throw new IllegalStateException();
	}

	public byte systemType() {
		return _delegate.systemType();
	}

	public int totalFreespace() {
		return _blockConverter.blocksToBytes(_delegate.totalFreespace());
	}

	public void traverse(final Visitor4<Slot> visitor) {
		_delegate.traverse(new Visitor4<Slot>() {
			public void visit(Slot slot) {
				visitor.visit(_blockConverter.toNonBlockedLength(slot));
			}
		});
	}

	public void write(LocalObjectContainer container) {
		_delegate.write(container);
	}

	public void slotFreed(Slot slot) {
		_delegate.slotFreed(slot);
	}

	public boolean isStarted() {
		return _delegate.isStarted();
	}

	public Slot allocateTransactionLogSlot(int length) {
		Slot slot = _delegate.allocateTransactionLogSlot(_blockConverter.bytesToBlocks(length));
		if(slot == null){
			return null;
		}
		return _blockConverter.toNonBlockedLength(slot);
	}

	public void read(LocalObjectContainer container, Slot slot) {
		throw new IllegalStateException();
	}

}

