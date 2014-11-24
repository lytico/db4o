/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.internal.ids.*;
import com.db4o.internal.slots.*;


/**
 * @exclude
 */
public class PersistentIntegerArray extends LocalPersistentBase {
	
	
	private final SlotChangeFactory _slotChangeFactory;
	
	private int[] _ints;
	
	public PersistentIntegerArray(SlotChangeFactory slotChangeFactory, TransactionalIdSystem idSystem, int[] arr){
		super(idSystem);
		_slotChangeFactory = slotChangeFactory;
		_ints = new int[arr.length];
		System.arraycopy(arr, 0, _ints, 0, arr.length);
	}
	
	public PersistentIntegerArray(SlotChangeFactory slotChangeFactory, TransactionalIdSystem idSystem, int id) {
		super(idSystem);
		_slotChangeFactory = slotChangeFactory;
		setID(id);
	}
	
	public byte getIdentifier() {
		return Const4.INTEGER_ARRAY;
	}

	public int ownLength() {
		return (Const4.INT_LENGTH * (size() + 1)) + Const4.ADDED_LENGTH;
	}

	public void readThis(Transaction trans, ByteArrayBuffer reader) {
		int length = reader.readInt();
		_ints = new int[length];
		for (int i = 0; i < length; i++) {
			_ints[i] = reader.readInt();
		}
	}

	public void writeThis(Transaction trans, ByteArrayBuffer writer) {
		writer.writeInt(size());
		for (int i = 0; i < _ints.length; i++) {
			writer.writeInt(_ints[i]);
		}
	}
	
	private int size(){
		return _ints.length;
	}
	
	public int[] array(){
		return _ints;
	}
	
	@Override
	public SlotChangeFactory slotChangeFactory() {
		return _slotChangeFactory;
	}

}
