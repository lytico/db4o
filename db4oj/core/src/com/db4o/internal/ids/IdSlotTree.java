/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.ids;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public class IdSlotTree extends TreeInt {
	
	private final Slot _slot;

	public IdSlotTree(int id, Slot slot) {
		super(id);
		_slot = slot;
	}

	public Slot slot() {
		return _slot;
	}
	
	@Override
	public Tree onAttemptToAddDuplicate(Tree oldNode) {
		_preceding = oldNode._preceding;
		_subsequent = oldNode._subsequent;
		_size = oldNode._size;
		return this;
	}
	
	@Override
	public int ownLength() {
		return Const4.INT_LENGTH * 3;   // _key, _slot._address, _slot._length 
	}
	
	@Override
	public Object read(ByteArrayBuffer buffer) {
		int id = buffer.readInt();
		Slot slot = new Slot(buffer.readInt(), buffer.readInt());
		return new IdSlotTree(id, slot);
	}
	
	@Override
	public void write(ByteArrayBuffer buffer) {
		buffer.writeInt(_key);
		buffer.writeInt(_slot.address());
		buffer.writeInt(_slot.length());
	}

}
