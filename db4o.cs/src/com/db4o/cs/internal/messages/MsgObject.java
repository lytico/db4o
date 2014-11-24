/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.internal.*;
import com.db4o.internal.slots.*;


public class MsgObject extends MsgD {
	
	private static final int LENGTH_FOR_ALL = Const4.ID_LENGTH + (Const4.INT_LENGTH * 2);
	private static final int LENGTH_FOR_FIRST = LENGTH_FOR_ALL;
	
	private int _id;
	private int _address;
	
	final MsgD getWriter(Transaction trans, Pointer4 pointer, ByteArrayBuffer buffer,int[] prependInts) {
		int lengthNeeded = buffer.length() + LENGTH_FOR_FIRST;
		if(prependInts != null){
			lengthNeeded += (prependInts.length * Const4.INT_LENGTH);
		}
		MsgD message = getWriterForLength(trans, lengthNeeded);
		if(prependInts != null){
		    for (int i = 0; i < prependInts.length; i++) {
		        message._payLoad.writeInt(prependInts[i]);    
            }
		}
		appendPayLoad(message._payLoad, pointer, buffer);
		return message;
	}
	
    private void appendPayLoad(StatefulBuffer target, Pointer4 pointer, final ByteArrayBuffer payLoad) {
        target.writeInt(payLoad.length());
        target.writeInt(pointer.id());
        target.writeInt(pointer.address());
        target.append(payLoad._buffer);
    }


	final public MsgD getWriter(StatefulBuffer buffer) {
		return getWriter(buffer.transaction(), buffer.pointer(), buffer, null);
	}
	
	public final MsgD getWriter(Transaction trans, Pointer4 pointer, ClassMetadata classMetadata, ByteArrayBuffer buffer) {
        if(classMetadata == null){
            return getWriter(trans, pointer, buffer, new int[]{0});
        }
		return getWriter(trans, pointer, buffer, new int[]{ classMetadata.getID()});
	}
	
	public final MsgD getWriter(Transaction trans, Pointer4 pointer, ClassMetadata classMetadata, int param, ByteArrayBuffer buffer) {
		return getWriter(trans, pointer, buffer, new int[]{ classMetadata.getID(), param});
	}
	
	public final StatefulBuffer unmarshall() {
		return unmarshall(0);
	}

	public final StatefulBuffer unmarshall(int addLengthBeforeFirst) {
		_payLoad.setTransaction(transaction());
		
		int length = _payLoad.readInt();
		if (length == 0) {
			return null;  // does this happen ? Yes it does. Confirmed.
		}
		_id = _payLoad.readInt();
		_address = _payLoad.readInt();
		_payLoad.removeFirstBytes(LENGTH_FOR_FIRST + addLengthBeforeFirst);
		_payLoad.useSlot(_id, _address, length);
		return _payLoad;
	}

	public int getId() {
		return _id;
	}
}
